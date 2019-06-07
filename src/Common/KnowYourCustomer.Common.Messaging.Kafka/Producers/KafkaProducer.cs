using Confluent.Kafka;
using KnowYourCustomer.Common.Messaging.Extensions;
using KnowYourCustomer.Common.Messaging.Interfaces;
using KnowYourCustomer.Common.Messaging.Kafka.Serializers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KnowYourCustomer.Common.Messaging.Kafka.Producers
{
    public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        private readonly IProducer<TKey, TValue> _producer;

        private readonly IKafkaOptions _kafkaOptions;
        private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;

        public string Topic { get; set; }

        internal KafkaProducer(IKafkaOptions kafkaOptions, ILogger<KafkaProducer<TKey, TValue>> logger)
        {
            _kafkaOptions = Guard.IsNotNull(kafkaOptions, nameof(kafkaOptions));
            _logger = Guard.IsNotNull(logger, nameof(logger));

            var config = new ProducerConfig(_kafkaOptions.AdditionalConfiguration)
            {
                BootstrapServers = _kafkaOptions.BootstrapServers,
                MessageTimeoutMs = _kafkaOptions.MessageTimeoutMs,
                SaslUsername = _kafkaOptions.SaslUsername,
                SaslPassword = _kafkaOptions.SaslPassword,
                SecurityProtocol = _kafkaOptions.SecurityProtocol,
                SaslMechanism = _kafkaOptions.SaslMechanism
            };

            _producer = new ProducerBuilder<TKey, TValue>(config)
                .SetValueSerializer(new JsonSerializer<TValue>())
                .SetErrorHandler((_, e) => _logger.LogError(e.Reason))
                .SetLogHandler((_, e) => _logger.LogInformation(e.Message))
                .Build();
        }

        public async Task ProduceAsync(IKafkaMessage<TKey, TValue> kafkaMessage)
        {
            kafkaMessage.Headers.AddOrUpdate(KafkaConstants.MessageTypeHeaderName, kafkaMessage.MessageType);

            var message = new Message<TKey, TValue>
            {
                Key = kafkaMessage.Key,
                Value = kafkaMessage.Value,
                Timestamp = new Timestamp(DateTime.UtcNow),
                Headers = kafkaMessage.Headers.ToKafkaHeaders()
            };

            await ProduceAsync(message);
        }

        private async Task ProduceAsync(Message<TKey, TValue> message)
        {
            try
            {
                if(Topic == null)
                {
                    throw new Exception("Topic cannot be null");
                }

                var deliveryReport = await _producer.ProduceAsync(Topic, message);
                _logger.LogInformation($"Delivered 'key: { deliveryReport.Key }' - '{ deliveryReport.Value }' to '{ deliveryReport.TopicPartitionOffset }' with offset '{ deliveryReport.Offset }'");
            }
            catch (ProduceException<TKey, TValue> e)
            {
                _logger.LogError(e, $"Delivery failed: { e.Error.Reason }");
                throw;
            }
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(5));
            _producer.Dispose();
        }
    }
}