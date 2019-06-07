using KnowYourCustomer.Kyc.Data.Contracts.Entities;
using System;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Data.Contracts.Interfaces
{
    public interface IKycOperationRepository
    {
        Task AddAsync(KycOperationEntity entity);
        Task<KycOperationEntity> GetByKycIdAsync(Guid kycId);
        Task<KycOperationEntity> GetByUserIdAsync(Guid userId);
    }
}