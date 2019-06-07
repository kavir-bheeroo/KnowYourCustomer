using System;

namespace KnowYourCustomer.Kyc.Contracts.Models
{
    public class InitiateKycRequestModel
    {
        public Guid UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}