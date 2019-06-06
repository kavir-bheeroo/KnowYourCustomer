using KnowYourCustomer.Common;
using KnowYourCustomer.Kyc.Data.Contracts.Entities;
using KnowYourCustomer.Kyc.Data.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Data.EfCore.Repositories
{
    public class KycRepository : IKycRepository
    {
        private readonly KycDbContext _dbContext;

        public KycRepository(KycDbContext dbContext)
        {
            _dbContext = Guard.IsNotNull(dbContext, nameof(dbContext));
        }

        public async Task AddAsync(KycEntity entity)
        {
            await _dbContext.Kyc.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<KycEntity> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Kyc.AsNoTracking().FirstOrDefaultAsync(kyc => kyc.UserId.Equals(userId));
        }

        public async Task UpdateAsync(KycEntity entity)
        {
            _dbContext.Kyc.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}