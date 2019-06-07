using System;

namespace KnowYourCustomer.Kyc.Contracts.Models
{
    public class CheckMrzStatusResponseModel
    {
        public UserInfo UserInfo { get; set; }
        public PassportInfo PassportInfo { get; set; }
    }

    public class UserInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Id { get; set; }
        public string Nationality { get; set; }
    }

    public class PassportInfo
    {
        public string Mrz1 { get; set; }
        public string Mrz2 { get; set; }
        public string Number { get; set; }
        public DateTime DateOfExpiry { get; set; }
    }
}