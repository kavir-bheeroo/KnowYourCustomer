using System;
using System.Threading;

namespace KnowYourCustomer.Common.Messaging.Interfaces
{
    public interface IKafkaConsumer<TKey, TValue> : IDisposable
    {
        string Topic { get; set; }

        void Subscribe();

        IKafkaMessage<TKey, TValue> Consume(CancellationToken token = default(CancellationToken));

        void Commit(CancellationToken token = default(CancellationToken));
    }
}