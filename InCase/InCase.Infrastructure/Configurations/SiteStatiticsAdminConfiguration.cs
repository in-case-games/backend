using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class SiteStatiticsAdminConfiguration : BaseEntityConfiguration<SiteStatiticsAdmin>
    {
        public override void Configure(EntityTypeBuilder<SiteStatiticsAdmin> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(SiteStatiticsAdmin));

            builder.Property(p => p.BalanceWithdrawn)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();
            builder.Property(p => p.TotalReplenished)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();
            builder.Property(p => p.SentSites)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();
        }
    }
}
