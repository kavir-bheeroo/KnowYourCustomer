using Confluent.Kafka;
using KnowYourCustomer.Common.Messaging.Interfaces;
using KnowYourCustomer.Common.Messaging.Kafka.Serializers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KnowYourCustomer.Common.Messaging.Kafka.Consumers
{
    public class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TKey, TValue>
    {
        private readonly IConsumer<TKey, TValue> _consumer;

        private readonly IKafkaOptions _kafkaOptions;
        private readonly ILogger<KafkaConsumer<TKey, TValue>> _logger;

        private bool _disposed = false;

        public KafkaConsumer(IKafkaOptions kafkaOptions, ILogger<KafkaConsumer<TKey, TValue>> logger)
        {
            _kafkaOptions = Guard.IsNotNull(kafkaOptions, nameof(kafkaOptions));
            _logger = Guard.IsNotNull(logger, nameof(logger));

            var config = new ConsumerConfig(_kafkaOptions.AdditionalConfiguration)
            {
                BootstrapServers = _kafkaOptions.BootstrapServers,
                GroupId = _kafkaOptions.GroupId,
                AutoOffsetReset = _kafkaOptions.AutoOffsetReset,
                EnableAutoCommit = _kafkaOptions.EnableAutoCommit,
                SaslUsername = _kafkaOptions.SaslUsername,
                SaslPassword = _kafkaOptions.SaslPassword,
                SecurityProtocol = _kafkaOptions.SecurityProtocol,
                SaslMechanism = _kafkaOptions.SaslMechanism
            };

            _consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetValueDeserializer(new JsonDeserializer<TValue>())
                .SetErrorHandler((_, e) => _logger.LogError(e.Reason))
                .SetLogHandler((_, e) => _logger.LogInformation(e.Message))
                .SetPartitionsAssignedHandler((_, e) => _logger.LogDebug($"Assigned partitions: [{string.Join(", ", e.Select(x => x.Partition))}]"))
                .SetPartitionsRevokedHandler((_, e) => _logger.LogDebug($"Revoked partitions: [{string.Join(", ", e.Select(x => x.Partition))}]"))
                .Build();

            _consumer.Subscribe(_kafkaOptions.Topic);
        }

        public IKafkaMessage<TKey, TValue> Consume(CancellationToken token = default(CancellationToken))
        {
            try
            {
                var consumeResult = _consumer.Consume(token);
                var kafkaMessage = (KafkaMessage<TKey, TValue>)consumeResult;

                _logger.LogDebug($"Consumed message 'key { consumeResult.Key } ' at: '{ consumeResult.TopicPartitionOffset }'.");

                return kafkaMessage;
            }
            catch (ConsumeException e)
            {
                _logger.LogError(e, $"ConsumeException occurred: { e.Error.Reason }");
                throw;
            }
            catch (KafkaException e)
            {
                _logger.LogError(e, $"KafkaException occurred: { e.Error.Reason }");
                throw;
            }
        }

        public void Commit(IKafkaMessage<TKey, TValue> message, CancellationToken token = default(CancellationToken))
        {
            var kafkaMessage = (KafkaMessage<TKey, TValue>)message;
            _consumer.Commit(new List<TopicPartitionOffset> { kafkaMessage.Offset });
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _logger.LogInformation("Disposing...");

                _consumer.Close();
                _consumer.Dispose();

                _disposed = true;
            }
        }
    }
}