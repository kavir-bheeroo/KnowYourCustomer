using System;

namespace KnowYourCustomer.Kyc.Contracts.Models
{
    public class CheckMrzStatusRequestModel
    {
        public Guid UserId { get; set; }
        public Guid KycId { get; set; }
        public string TaskId { get; set; }
    }
}