using System;

namespace KnowYourCustomer.Kyc.Contracts.Models
{
    public class UserInfoModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Id { get; set; }
        public string Nationality { get; set; }
    }
}
