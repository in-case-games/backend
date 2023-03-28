using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class SiteStatiticsConfiguration : BaseEntityConfiguration<SiteStatitics>
    {
        public override void Configure(EntityTypeBuilder<SiteStatitics> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(SiteStatitics));

            builder.Property(p => p.Users)
                .IsRequired();
            builder.Property(p => p.Reviews)
                .IsRequired();
            builder.Property(p => p.OpenCases)
                .IsRequired();
            builder.Property(p => p.WithdrawnItems)
                .IsRequired();
            builder.Property(p => p.WithdrawnFunds)
                .IsRequired();
        }
    }
}
