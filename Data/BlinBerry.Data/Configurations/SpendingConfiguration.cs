using System;
using System.Collections.Generic;
using System.Text;
using BlinBerry.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlinBerry.Data.Configurations
{
    public class SpendingConfiguration : AuditableEntityConfiguration<Spending>
    {
        public override void Configure(EntityTypeBuilder<Spending> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Money).IsRequired();
            builder.Property(x => x.Comment).HasColumnType("NVARCHAR(200)").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Date).IsRequired();

        }
    }
}
