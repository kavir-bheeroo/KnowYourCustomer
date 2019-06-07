using System;

namespace KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models
{
    public class MrzStatusRequest
    {
        public Guid KycId { get; set; }
        public string TaskId { get; set; }
    }
}