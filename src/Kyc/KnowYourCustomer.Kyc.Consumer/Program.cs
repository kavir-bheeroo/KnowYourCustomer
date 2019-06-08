﻿using AutoMapper;
using KnowYourCustomer.Common.Hosting;
using KnowYourCustomer.Common.Messaging.Kafka.Extensions;
using KnowYourCustomer.Kyc.Consumer.Mappers;
using KnowYourCustomer.Kyc.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http.Headers;

namespace KnowYourCustomer.Kyc.Consumer
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            ConsoleHostHelper.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient("kyc", c =>
                    {
                        c.BaseAddress = new Uri(hostContext.Configuration["KycServiceUrl"]);
                        c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    });

                    services.AddHttpClient("identity", c =>
                    {
                        c.BaseAddress = new Uri(hostContext.Configuration["IdentityServerUrl"]);
                        c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    });

                    services.AddAutoMapper(typeof(MappingProfile));

                    services
                        .AddKafkaConsumer<string, InitiateKycResponseModel>(hostContext.Configuration, "initiate-kyc")
                        .AddHostedService<InitiateKycService>()
                        .AddKafkaConsumer<string, CheckMrzStatusResponseModel>(hostContext.Configuration, "check-mrz")
                        .AddHostedService<CheckMrzStatusService>()
                        .AddKafkaConsumer<string, VerificationResponseModel>(hostContext.Configuration, "verify-identity")
                        .AddHostedService<VerifyIdentityService>();
                });
    }
}