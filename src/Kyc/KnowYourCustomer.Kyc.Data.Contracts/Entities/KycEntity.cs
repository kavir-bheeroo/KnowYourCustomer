using System;
using System.Collections.Generic;
using System.Text;

namespace KnowYourCustomer.Kyc.Data.Contracts.Entities
{
    public class KycEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public KycStatus Status { get; set; }
        public DateTime KycRequestDate { get; set; }
    }
}