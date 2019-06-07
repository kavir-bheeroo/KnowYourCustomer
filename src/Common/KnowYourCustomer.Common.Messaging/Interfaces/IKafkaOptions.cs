using Confluent.Kafka;
using System.Collections.Generic;

namespace KnowYourCustomer.Common.Messaging.Interfaces
{
    public interface IKafkaOptions
    {
        string BootstrapServers { get; set; }

        string GroupId { get; set; }

        string Topic { get; set; }

        int MessageTimeoutMs { get; set; }

        AutoOffsetReset AutoOffsetReset { get; set; }

        bool EnableAutoCommit { get; set; }

        string SaslUsername { get; set; }

        string SaslPassword { get; set; }

        SecurityProtocol SecurityProtocol { get; set; }

        SaslMechanism SaslMechanism { get; set; }

        IDictionary<string, string> AdditionalConfiguration { get; set; }
    }
}