using System;
using System.Threading;

namespace KnowYourCustomer.Common.Messaging.Interfaces
{
    public interface IKafkaConsumer<TKey, TValue> : IDisposable
    {
        IKafkaMessage<TKey, TValue> Consume(CancellationToken token = default(CancellationToken));

        void Commit(IKafkaMessage<TKey, TValue> message, CancellationToken token = default(CancellationToken));
    }
}