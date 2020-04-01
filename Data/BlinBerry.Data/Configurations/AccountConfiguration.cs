using System;
using System.Collections.Generic;
using System.Text;
using BlinBerry.Data.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlinBerry.Data.Configurations
{
    public class AccountConfiguration : AuditableEntityConfiguration<CommonMoneyAndProducts>
    {
        public override void Configure(EntityTypeBuilder<CommonMoneyAndProducts> builder)
        {
            base.Configure(builder);
            builder.Property(x => x. TotalCash).IsRequired(false);
            builder.HasMany(x => x.Procurements).WithOne(x => x.BlinBerry).HasForeignKey(x => x.BlinBerryId).IsRequired();
            builder.HasMany(x => x.Reports).WithOne(x => x.BlinBerry).HasForeignKey(x => x.BlinBerryId).IsRequired();
        }
    }
}
