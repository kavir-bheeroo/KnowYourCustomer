using System.Collections.Generic;

namespace KnowYourCustomer.Common.Messaging.Interfaces
{
    public interface IKafkaMessage<TKey, TValue>
    {
        TKey Key { get; set; }

        TValue Value { get; set; }

        string MessageType { get; set; }

        IDictionary<string, string> Headers { get; set; }
    }
}