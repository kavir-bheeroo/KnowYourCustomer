using System;

namespace KnowYourCustomer.Kyc.Data.Contracts.Entities
{
    public class KycDocumentEntity
    {
        public Guid Id { get; set; }
        public Guid KycId { get; set; }
        public Guid UserId { get; set; }
        public byte[] Document { get; set; }
        public DateTime UploadedOn { get; set; }
    }
}