using KnowYourCustomer.Kyc.Data.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowYourCustomer.Kyc.Data.EfCore
{
    public class KycDbContext : DbContext
    {
        public DbSet<KycEntity> Kyc { get; set; }
        public DbSet<KycDocumentEntity> KycDocuments { get; set; }

        public KycDbContext(DbContextOptions<KycDbContext> options) : base(options) { }
    }
}