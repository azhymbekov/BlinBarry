using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlinBerry.Data.Common.Models;
using BlinBerry.Data.Models.Entities;
using BlinBerry.Data.Models.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlinBerry.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        
        public DbSet<ProductProcurement> ProductProcurements { get; set; }
        public DbSet<SelesReport> SelesReports { get; set; }
        public DbSet<Spending> Spendings { get; set; }
        public DbSet<CommonMoneyAndProducts> Account { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
          this.SaveChangesAsync(true, cancellationToken);

        public Task<int> SaveChangesAsync(Guid currentUserId, CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules(currentUserId);
            return base.SaveChangesAsync(true, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        private static void ConfigureUserIdentityRelations(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
        private void ApplyAuditInfoRules(Guid? currentUserId = null)
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo<ApplicationUser> &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo<ApplicationUser>)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = DateTime.Now;
                    if (entity.CreatedById == null)
                    {
                        entity.CreatedById = currentUserId;
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                    if (entity.ModifiedById == null)
                    {
                        entity.ModifiedById = currentUserId;
                    }
                }
            }
        }
    }
}
