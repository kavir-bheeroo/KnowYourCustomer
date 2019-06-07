using Confluent.Kafka;
using System.Collections.Generic;
using System.Text;

namespace KnowYourCustomer.Common.Messaging.Extensions
{
    public static class HeadersExtensions
    {
        private static Encoding encoding = Encoding.UTF8;

        public static string GetHeaderValue(this Headers headers, string headerName)
        {
            return headers.TryGetLastBytes(headerName, out var headerInBytes) ? encoding.GetString(headerInBytes) : null;
        }

        public static IDictionary<string, string> AsDictionary(this Headers headers)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var header in headers)
            {
                var headerValue = encoding.GetString(header.GetValueBytes());
                dictionary.Add(header.Key, headerValue);
            }

            return dictionary;
        }

        public static void Add(this Headers headers, string key, string value)
        {
            var byteValue = value == null ? null : encoding.GetBytes(value);
            headers.Add(key, byteValue);
        }
    }
}