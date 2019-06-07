using Confluent.Kafka;
using System.Collections.Generic;

namespace KnowYourCustomer.Common.Messaging.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static Headers ToKafkaHeaders(this IDictionary<string, string> dictionary)
        {
            var headers = new Headers();

            // Add headers
            if (dictionary != null)
            {
                foreach (var kvp in dictionary)
                {
                    headers.Add(kvp.Key, kvp.Value);
                }
            }

            return headers;
        }
    }
}
