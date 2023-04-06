using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class SiteStatiticsConfiguration : BaseEntityConfiguration<SiteStatistics>
    {
        public override void Configure(EntityTypeBuilder<SiteStatistics> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(SiteStatistics));

            builder.Property(p => p.Users)
                .IsRequired();
            builder.Property(p => p.Reviews)
                .IsRequired();
            builder.Property(p => p.LootBoxes)
                .IsRequired();
            builder.Property(p => p.WithdrawnItems)
                .IsRequired();
            builder.Property(p => p.WithdrawnFunds)
                .IsRequired();
        }
    }
}
