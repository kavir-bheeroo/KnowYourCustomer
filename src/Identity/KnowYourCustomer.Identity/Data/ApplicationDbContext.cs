using KnowYourCustomer.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace KnowYourCustomer.Identity.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }
        public DbSet<IdentityUserRole<Guid>> IdentityUserRoles { get; set; }
        public DbSet<IdentityRole<Guid>> IdentityRoles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<Guid>>().HasKey("UserId", "RoleId");

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}