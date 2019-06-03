using System.IO;

namespace KnowYourCustomer.Kyc.Contracts.Models
{
    public class KycFile
    {
        public string UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}