using System;

namespace KnowYourCustomer.Kyc.Contracts.Public.Models
{
    public class CheckMrzStatusRequest
    {
        public Guid KycId { get; set; }
        public string TaskId { get; set; }
    }
}