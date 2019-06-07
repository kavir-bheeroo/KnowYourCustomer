using Confluent.Kafka;
using KnowYourCustomer.Common.Messaging.Extensions;
using KnowYourCustomer.Common.Messaging.Interfaces;
using System.Collections.Generic;

namespace KnowYourCustomer.Common.Messaging.Kafka
{
    public class KafkaMessage<TKey, TValue> : IKafkaMessage<TKey, TValue>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public string MessageType { get; set; }

        internal TopicPartitionOffset Offset { get; set; }

        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public static implicit operator KafkaMessage<TKey, TValue>(ConsumeResult<TKey, TValue> result)
        {
            return new KafkaMessage<TKey, TValue>
            {
                Key = result.Message.Key,
                Value = result.Message.Value,
                MessageType = result.Message.Headers.GetHeaderValue(KafkaConstants.MessageTypeHeaderName),
                Headers = result.Message.Headers.AsDictionary(),
                Offset = result.TopicPartitionOffset
            };
        }
    }
}