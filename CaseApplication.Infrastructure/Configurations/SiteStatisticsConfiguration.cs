﻿using CaseApplication.Domain.Entities.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseApplication.Infrastructure.Configurations
{
    internal class SiteStatisticsConfiguration: BaseEntityConfiguration<SiteStatistics>
    {
        public override void Configure(EntityTypeBuilder<SiteStatistics> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.SiteBalance)
                .HasColumnType("DECIMAL(18, 5)")
                .IsRequired();
        }
    }
}
