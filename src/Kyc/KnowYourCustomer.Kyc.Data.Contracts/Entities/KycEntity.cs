using KnowYourCustomer.Kyc.Data.Contracts.Enumerations;
using System;

namespace KnowYourCustomer.Kyc.Data.Contracts.Entities
{
    public class KycEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public KycStatus Status { get; set; }
        public DateTime RequestDate { get; set; }

        public KycEntity() { }

        public KycEntity(Guid userId, KycStatus status)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Status = status;
            RequestDate = DateTime.UtcNow;
        }
    }
}