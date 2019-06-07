using KnowYourCustomer.Kyc.Data.Contracts.Entities;
using System;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Data.Contracts.Interfaces
{
    public interface IKycRepository
    {
        Task<KycEntity> AddAsync(KycEntity entity);
        Task<KycEntity> GetByUserIdAsync(Guid userId);
        Task<KycEntity> GetByKycIdAsync(Guid kycId);
        Task UpdateAsync(KycEntity entity);
    }
}