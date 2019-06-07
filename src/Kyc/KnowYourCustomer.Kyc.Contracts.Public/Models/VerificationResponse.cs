using System;
using System.Collections.Generic;
using System.Text;

namespace KnowYourCustomer.Kyc.Contracts.Public.Models
{
    public class VerificationResponse
    {
        public Guid UserId { get; set; }
        public UserInfo UserInfo { get; set; }
        public PassportInfo PassportInfo { get; set; }
        public bool IsVerified { get; set; }
    }
}