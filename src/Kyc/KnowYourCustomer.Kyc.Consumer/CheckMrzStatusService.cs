using AutoMapper;
using KnowYourCustomer.Common;
using KnowYourCustomer.Common.Http.Interfaces;
using KnowYourCustomer.Common.Messaging.Interfaces;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.Contracts.Public.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Consumer
{
    public class CheckMrzStatusService : IHostedService
    {
        private readonly IKafkaConsumer<string, CheckMrzStatusResponseModel> _consumer;

        private readonly IIdentityServerClient _identityServerClient;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public CheckMrzStatusService(
            IKafkaConsumer<string, CheckMrzStatusResponseModel> consumer,
            IIdentityServerClient identityServerClient,
            IHttpClientFactory httpClientFactory,
            IMapper mapper)
        {
            _consumer = Guard.IsNotNull(consumer, nameof(consumer));
            _identityServerClient = Guard.IsNotNull(identityServerClient, nameof(identityServerClient));
            Guard.IsNotNull(httpClientFactory, nameof(httpClientFactory));
            _httpClient = httpClientFactory.CreateClient("kyc");
            _mapper = Guard.IsNotNull(mapper, nameof(mapper));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(() => RunConsumerAsync(cancellationToken), cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Dispose();
            return Task.CompletedTask;
        }

        private async Task RunConsumerAsync(CancellationToken token)
        {
            try
            {
                _consumer.Subscribe();

                while (true)
                {
                    try
                    {
                        var message = _consumer.Consume(token);
                        var handlerCompletionResponse = await HandleMessage(message);
                        if (handlerCompletionResponse)
                        {
                            _consumer.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error occurred: { e.Message } ");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Dispose();
            }
        }

        private async Task<bool> HandleMessage(IKafkaMessage<string, CheckMrzStatusResponseModel> message)
        {
            var serializedValue = JsonConvert.SerializeObject(message.Value);
            Console.WriteLine($"Consumed message in service \nkey: '{ message.Key }' \nvalue: '{ serializedValue }' at {DateTime.UtcNow}");

            foreach (var header in message.Headers)
            {
                Console.WriteLine($"Key: { header.Key }\tValue: { header.Value }");
            }

            try
            {
                var request = _mapper.Map<VerificationRequest>(message.Value);
                var payload = JsonConvert.SerializeObject(request);

                // Get token from Identity Server
                var accessToken = await _identityServerClient.RequestClientCredentialsTokenAsync();

                // Set token in http client
                _httpClient.SetBearerToken(accessToken);

                var response = await _httpClient.PostAsync("api/kyc/verifyidentity", new StringContent(payload, Encoding.UTF8, "application/json"));

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}