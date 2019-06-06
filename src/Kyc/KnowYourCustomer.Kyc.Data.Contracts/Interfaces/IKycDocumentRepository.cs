using KnowYourCustomer.Kyc.Data.Contracts.Entities;
using System;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Data.Contracts.Interfaces
{
    public interface IKycDocumentRepository
    {
        Task AddAsync(KycDocumentEntity entity);
        Task<KycDocumentEntity> GetByKycIdAsync(Guid kycId);
        Task<KycDocumentEntity> GetByUserIdAsync(Guid userId);
    }
}