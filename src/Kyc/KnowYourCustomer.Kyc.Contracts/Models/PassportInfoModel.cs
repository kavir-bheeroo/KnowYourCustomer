using System;

namespace KnowYourCustomer.Kyc.Contracts.Models
{
    public class PassportInfoModel
    {
        public string Mrz1 { get; set; }
        public string Mrz2 { get; set; }
        public string Number { get; set; }
        public DateTime DateOfExpiry { get; set; }
    }
}