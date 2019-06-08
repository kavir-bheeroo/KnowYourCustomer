using KnowYourCustomer.Common.Messaging.Interfaces;
using KnowYourCustomer.Common.Messaging.Kafka.Producers;
using Microsoft.Extensions.Logging;

namespace KnowYourCustomer.Common.Messaging.Kafka.Factories
{
    public class ProducerFactory : IProducerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public ProducerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = Guard.IsNotNull(loggerFactory, nameof(loggerFactory));
        }

        public IKafkaProducer<TKey, TValue> Create<TKey, TValue>(IKafkaOptions kafkaOptions, string topic)
        {
            var producer = new KafkaProducer<TKey, TValue>(kafkaOptions, _loggerFactory.CreateLogger<KafkaProducer<TKey, TValue>>())
            {
                Topic = topic
            };

            return producer;
        }
    }
}