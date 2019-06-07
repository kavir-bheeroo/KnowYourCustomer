using System;
using System.Threading.Tasks;

namespace KnowYourCustomer.Common.Messaging.Interfaces
{
    public interface IKafkaProducer<TKey, TValue> : IDisposable
    {
        string Topic { get; set; }
        Task ProduceAsync(IKafkaMessage<TKey, TValue> kafkaMessage);
    }
}