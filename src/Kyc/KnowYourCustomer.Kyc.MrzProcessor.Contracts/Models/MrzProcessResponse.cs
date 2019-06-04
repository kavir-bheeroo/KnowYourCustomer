using System.Collections.Generic;

namespace KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models
{
    public class MrzProcessResponse
    {
        public IDictionary<string, string> ReceivedDetails { get; set; }

        public MrzProcessResponse(IDictionary<string, string> dictionary)
        {
            ReceivedDetails = new Dictionary<string, string>();

            foreach (var item in dictionary)
            {
                ReceivedDetails.Add(item.Key.ToLower(), item.Value);
            }
        }
    }
}