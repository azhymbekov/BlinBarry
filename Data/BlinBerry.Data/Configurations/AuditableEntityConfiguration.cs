using System;
using System.Collections.Generic;
using System.Text;
using BlinBerry.Data.Common.Models;
using BlinBerry.Data.Models.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlinBerry.Data.Configurations
{
    public class AuditableEntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : class, IAuditInfo<ApplicationUser>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById).IsRequired(false);
            builder.HasOne(x => x.ModifiedBy).WithMany().HasForeignKey(x => x.ModifiedById).IsRequired(false);
        }
    }
}
