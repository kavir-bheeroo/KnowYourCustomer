namespace KnowYourCustomer.Common.Messaging.Interfaces
{
    public interface IConsumerFactory
    {
        IKafkaConsumer<TKey, TValue> Create<TKey, TValue>(IKafkaOptions kafkaOptions, string topic);
    }
}