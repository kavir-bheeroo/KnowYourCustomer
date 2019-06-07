using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

namespace KnowYourCustomer.Common.Messaging.Kafka.Serializers
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            if (data == null)
            {
                return null;
            }

            var serializedDataAsJson = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(serializedDataAsJson);
        }
    }
}