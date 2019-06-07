using KnowYourCustomer.Common;
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
    public class InitiateKycService : IHostedService
    {
        private readonly IKafkaConsumer<string, InitiateKycResponseModel> _consumer;
        private readonly HttpClient _httpClient;

        public InitiateKycService(IKafkaConsumer<string, InitiateKycResponseModel> consumer, IHttpClientFactory httpClientFactory)
        {
            _consumer = Guard.IsNotNull(consumer, nameof(consumer));
            Guard.IsNotNull(httpClientFactory, nameof(httpClientFactory));
            _httpClient = httpClientFactory.CreateClient("kyc");
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

        private async Task<bool> HandleMessage(IKafkaMessage<string, InitiateKycResponseModel> message)
        {
            var serializedValue = JsonConvert.SerializeObject(message.Value);
            Console.WriteLine($"Consumed message in service \nkey: '{ message.Key }' \nvalue: '{ serializedValue }' at {DateTime.UtcNow}");

            foreach (var header in message.Headers)
            {
                Console.WriteLine($"Key: { header.Key }\tValue: { header.Value }");
            }

            try
            {
                var request = new CheckMrzStatusRequest { UserId = message.Value.UserId, KycId = message.Value.KycId, TaskId = message.Value.MrzTaskId };
                var payload = JsonConvert.SerializeObject(request);
                var response = await _httpClient.PostAsync("api/kyc/checkmrzstatus", new StringContent(payload, Encoding.UTF8, "application/json"));

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