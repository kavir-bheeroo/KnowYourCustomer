using KnowYourCustomer.Common.Messaging.Interfaces;
using KnowYourCustomer.Common.Messaging.Kafka.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KnowYourCustomer.Common.Messaging.Kafka.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services, IConfiguration configuration, string topic)
        {
            Guard.IsNotNull(services, nameof(services));

            var kafkaOptions = configuration.GetSection(KafkaOptions.DefaultSectionName).Get<KafkaOptions>();

            services
                .AddOptions()
                .TryAddSingleton<IKafkaOptions>(sp => kafkaOptions);

            services.TryAddSingleton<IProducerFactory, ProducerFactory>();
            services.TryAddSingleton(x => x.GetRequiredService<IProducerFactory>().Create<TKey, TValue>(kafkaOptions, topic));

            return services;
        }

        public static IServiceCollection AddKafkaConsumer<TKey, TValue>(this IServiceCollection services, IConfiguration configuration, string topic)
        {
            Guard.IsNotNull(services, nameof(services));

            var kafkaOptions = configuration.GetSection(KafkaOptions.DefaultSectionName).Get<KafkaOptions>();

            services
                .AddOptions()
                .TryAddSingleton<IKafkaOptions>(sp => kafkaOptions);

            services.TryAddSingleton<IConsumerFactory, ConsumerFactory>();
            services.TryAddSingleton(x => x.GetRequiredService<IConsumerFactory>().Create<TKey, TValue>(kafkaOptions, topic));

            return services;
        }
    }
}