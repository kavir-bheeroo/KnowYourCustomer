using System;

namespace KnowYourCustomer.Kyc.Contracts.Public.Models
{
    public class InitiateKycResponse
    {
        public Guid UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Guid KycId { get; set; }
        public string MrzTaskId { get; set; }
    }
}