using KnowYourCustomer.Common;
using KnowYourCustomer.Kyc.Data.Contracts.Entities;
using KnowYourCustomer.Kyc.Data.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Data.EfCore.Repositories
{
    public class KycOperationRepository : IKycOperationRepository
    {
        private readonly KycDbContext _dbContext;

        public KycOperationRepository(KycDbContext dbContext)
        {
            _dbContext = Guard.IsNotNull(dbContext, nameof(dbContext));
        }

        public async Task AddAsync(KycOperationEntity entity)
        {
            await _dbContext.KycOperations.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<KycOperationEntity> GetByKycIdAsync(Guid kycId)
        {
            return await _dbContext.KycOperations.AsNoTracking().FirstOrDefaultAsync(kd => kd.KycId.Equals(kycId));
        }

        public async Task<KycOperationEntity> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.KycOperations.AsNoTracking().FirstOrDefaultAsync(kd => kd.UserId.Equals(userId));
        }
    }
}