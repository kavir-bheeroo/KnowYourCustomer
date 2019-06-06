using KnowYourCustomer.Kyc.Data.Contracts.Entities;
using System;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Data.Contracts.Interfaces
{
    public interface IKycRepository
    {
        Task AddAsync(KycEntity entity);
        Task<KycEntity> GetByUserIdAsync(Guid userId);
        Task UpdateAsync(KycEntity entity);
    }
}