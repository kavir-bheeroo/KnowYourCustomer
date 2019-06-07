using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Text;

namespace KnowYourCustomer.Common.Messaging.Kafka.Serializers
{
    public class JsonDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
            {
                return default(T);
            }

            var deserializedDataAsJson = Encoding.UTF8.GetString(data.ToArray());

            return JsonConvert.DeserializeObject<T>(deserializedDataAsJson);
        }
    }
}