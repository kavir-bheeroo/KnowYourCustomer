using System;
using System.Collections.Generic;
using System.Globalization;

namespace KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models
{
    public class MrzProcessResponse
    {
        public UserInfo UserInfo { get; set; }
        public PassportInfo PassportInfo { get; set; }

        private const string DateTimeFormat = "yyMMdd";

        public MrzProcessResponse(IDictionary<string, string> dictionary)
        {
            UserInfo = new UserInfo
            {
                FirstName = dictionary["GivenName"],
                LastName = dictionary["LastName"],
                Id = dictionary["PersonalNumber"],
                Gender = dictionary["Sex"],
                Nationality = dictionary["Nationality"],
                DateOfBirth = DateTime.ParseExact(dictionary["BirthDate"], DateTimeFormat, CultureInfo.InvariantCulture)
            };

            PassportInfo = new PassportInfo
            {
                Mrz1 = dictionary["Line1"],
                Mrz2 = dictionary["Line2"],
                Number = dictionary["DocumentNumber"],
                DateOfExpiry = DateTime.ParseExact(dictionary["ExpiryDate"], DateTimeFormat, CultureInfo.InvariantCulture)
            };
        }
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