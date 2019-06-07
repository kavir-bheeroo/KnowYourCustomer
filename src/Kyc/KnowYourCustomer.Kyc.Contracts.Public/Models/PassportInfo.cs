using System;

namespace KnowYourCustomer.Kyc.Contracts.Public.Models
{
    public class PassportInfo
    {
        public string Mrz1 { get; set; }
        public string Mrz2 { get; set; }
        public string Number { get; set; }
        public DateTime DateOfExpiry { get; set; }
    }
}