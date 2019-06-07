using KnowYourCustomer.Common.Hosting;
using KnowYourCustomer.Common.Messaging.Kafka.Extensions;
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

                    services
                        .AddKafkaConsumer<string, InitiateKycResponseModel>(hostContext.Configuration)
                        .AddHostedService<InitiateKycService>();
                });
    }
}