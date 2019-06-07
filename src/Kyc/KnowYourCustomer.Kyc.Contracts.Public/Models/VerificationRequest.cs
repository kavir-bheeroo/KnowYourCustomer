using System;

namespace KnowYourCustomer.Kyc.Contracts.Public.Models
{
    public class VerificationRequest
    {
        public Guid UserId { get; set; }
        public Guid KycId { get; set; }
        public UserInfo UserInfo { get; set; }
        public PassportInfo PassportInfo { get; set; }
    }
}