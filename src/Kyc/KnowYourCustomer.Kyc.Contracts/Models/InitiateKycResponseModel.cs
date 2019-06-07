using System;

namespace KnowYourCustomer.Kyc.Contracts.Models
{
    public class InitiateKycResponseModel
    {
        public Guid UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Guid KycId { get; set; }
        public string MrzTaskId { get; set; }
    }
}