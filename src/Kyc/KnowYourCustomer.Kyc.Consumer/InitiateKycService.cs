﻿using KnowYourCustomer.Common;
using KnowYourCustomer.Common.Messaging.Interfaces;
using KnowYourCustomer.Common.Web.Interfaces;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.Contracts.Public.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        private readonly IIdentityServerClient _identityServerClient;
        private readonly HttpClient _httpClient;
        private readonly ILogger<InitiateKycService> _logger;

        public InitiateKycService(
            IKafkaConsumer<string, InitiateKycResponseModel> consumer,
            IIdentityServerClient identityServerClient,
            IHttpClientFactory httpClientFactory,
            ILogger<InitiateKycService> logger)
        {
            _consumer = Guard.IsNotNull(consumer, nameof(consumer));
            _identityServerClient = Guard.IsNotNull(identityServerClient, nameof(identityServerClient));
            Guard.IsNotNull(httpClientFactory, nameof(httpClientFactory));
            _httpClient = httpClientFactory.CreateClient("kyc");
            _logger = Guard.IsNotNull(logger, nameof(logger));
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
                        _logger.LogError(e, e.Message);
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
            _logger.LogDebug($"Consumed message in service \nkey: '{ message.Key }' \nvalue: '{ serializedValue }' at {DateTime.UtcNow}");

            foreach (var header in message.Headers)
            {
                _logger.LogDebug($"Key: { header.Key }\tValue: { header.Value }");
            }

            try
            {
                var request = new CheckMrzStatusRequest { UserId = message.Value.UserId, KycId = message.Value.KycId, TaskId = message.Value.MrzTaskId };
                var payload = JsonConvert.SerializeObject(request);

                // Get token from Identity Server
                var accessToken = await _identityServerClient.RequestClientCredentialsTokenAsync();

                // Set token in http client
                _httpClient.SetBearerToken(accessToken);

                var response = await _httpClient.PostAsync("api/kyc/checkmrzstatus", new StringContent(payload, Encoding.UTF8, "application/json"));

                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            return false;
        }
    }
}