using System;

namespace KnowYourCustomer.Kyc.Contracts.Models
{
    public class VerificationResponseModel
    {
        public Guid UserId { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public PassportInfoModel PassportInfo { get; set; }
        public bool IsVerified { get; set; }
    }
}