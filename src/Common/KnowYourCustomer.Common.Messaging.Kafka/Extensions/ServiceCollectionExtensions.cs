using KnowYourCustomer.Common.Messaging.Interfaces;
using KnowYourCustomer.Common.Messaging.Kafka.Consumers;
using KnowYourCustomer.Common.Messaging.Kafka.Producers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KnowYourCustomer.Common.Messaging.Kafka.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services, IConfiguration configuration)
        {
            Guard.IsNotNull(services, nameof(services));

            var kafkaOptions = configuration.GetSection(KafkaOptions.DefaultSectionName).Get<KafkaOptions>();

            services
                .AddOptions()
                .TryAddSingleton<IKafkaOptions>(sp => kafkaOptions);

            services.TryAddSingleton<IKafkaProducer<TKey, TValue>, KafkaProducer<TKey, TValue>>();

            return services;
        }

        public static IServiceCollection AddKafkaConsumer<TKey, TValue>(this IServiceCollection services, IConfiguration configuration)
        {
            Guard.IsNotNull(services, nameof(services));

            var kafkaOptions = configuration.GetSection(KafkaOptions.DefaultSectionName).Get<KafkaOptions>();

            services
                .AddOptions()
                .TryAddSingleton<IKafkaOptions>(sp => kafkaOptions);

            services.TryAddSingleton<IKafkaConsumer<TKey, TValue>, KafkaConsumer<TKey, TValue>>();

            return services;
        }
    }
}