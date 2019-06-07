using System;

namespace KnowYourCustomer.Kyc.Contracts.Public.Models
{
    public class CheckMrzStatusRequest
    {
        public Guid UserId { get; set; }
        public Guid KycId { get; set; }
        public string TaskId { get; set; }
    }
}