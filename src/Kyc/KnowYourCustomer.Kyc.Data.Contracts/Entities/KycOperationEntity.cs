using KnowYourCustomer.Kyc.Data.Contracts.Enumerations;
using System;

namespace KnowYourCustomer.Kyc.Data.Contracts.Entities
{
    public class KycOperationEntity
    {
        public Guid Id { get; set; }
        public Guid KycId { get; set; }
        public Guid UserId { get; set; }
        public Operation Operation { get; set; }
        public Provider Provider { get; set; }
        public DateTime TimeOperation { get; set; }
    }
}