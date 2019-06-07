using KnowYourCustomer.Common.Messaging.Interfaces;
using KnowYourCustomer.Common.Messaging.Kafka.Consumers;
using Microsoft.Extensions.Logging;

namespace KnowYourCustomer.Common.Messaging.Kafka.Factories
{
    public class ConsumerFactory : IConsumerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public ConsumerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = Guard.IsNotNull(loggerFactory, nameof(loggerFactory));
        }

        public IKafkaConsumer<TKey, TValue> Create<TKey, TValue>(IKafkaOptions kafkaOptions, string topic)
        {
            var consumer = new KafkaConsumer<TKey, TValue>(kafkaOptions, _loggerFactory.CreateLogger<KafkaConsumer<TKey, TValue>>())
            {
                Topic = topic
            };

            return consumer;
        }
    }
}