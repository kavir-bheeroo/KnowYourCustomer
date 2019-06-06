using KnowYourCustomer.Common;
using KnowYourCustomer.Kyc.Data.Contracts.Entities;
using KnowYourCustomer.Kyc.Data.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Data.EfCore.Repositories
{
    public class KycDocumentRepository : IKycDocumentRepository
    {
        private readonly KycDbContext _dbContext;

        public KycDocumentRepository(KycDbContext dbContext)
        {
            _dbContext = Guard.IsNotNull(dbContext, nameof(dbContext));
        }

        public async Task AddAsync(KycDocumentEntity entity)
        {
            await _dbContext.KycDocuments.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<KycDocumentEntity> GetByKycIdAsync(Guid kycId)
        {
            return await _dbContext.KycDocuments.AsNoTracking().FirstOrDefaultAsync(kd => kd.KycId.Equals(kycId));
        }

        public async Task<KycDocumentEntity> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.KycDocuments.AsNoTracking().FirstOrDefaultAsync(kd => kd.UserId.Equals(userId));
        }
    }
}