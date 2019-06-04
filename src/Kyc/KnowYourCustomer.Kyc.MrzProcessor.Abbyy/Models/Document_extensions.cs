using System.Collections.Generic;

namespace KnowYourCustomer.Kyc.MrzProcessor.Abbyy.Models
{
    public partial class DocumentType
    {
        public IDictionary<string, string> ToDictionary()
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var item in Items)
            {
                dictionary.Add(item.type, item.value);
            }

            return dictionary;
        }
    }
}