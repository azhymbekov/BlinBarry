using System;
using System.Collections.Generic;
using System.Text;
using BlinBerry.Data.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlinBerry.Data.Configurations
{
    public class AccountConfiguration : AuditableEntityConfiguration<State>
    {
        public override void Configure(EntityTypeBuilder<State> builder)
        {
            base.Configure(builder);
            builder.Property(x => x. TotalCash).IsRequired();

        }
    }
}
