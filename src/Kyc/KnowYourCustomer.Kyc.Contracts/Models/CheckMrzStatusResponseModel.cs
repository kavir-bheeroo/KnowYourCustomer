using System;

namespace KnowYourCustomer.Kyc.Contracts.Models
{
    public class CheckMrzStatusResponseModel
    {
        public Guid UserId { get; set; }
        public Guid KycId { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public PassportInfoModel PassportInfo { get; set; }
    }
}