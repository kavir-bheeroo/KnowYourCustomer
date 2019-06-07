namespace KnowYourCustomer.Common.Messaging.Interfaces
{
    public interface IProducerFactory
    {
        IKafkaProducer<TKey, TValue> Create<TKey, TValue>(IKafkaOptions kafkaOptions, string topic);
    }
}