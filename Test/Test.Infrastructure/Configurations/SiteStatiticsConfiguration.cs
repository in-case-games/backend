using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class SiteStatiticsConfiguration : BaseEntityConfiguration<SiteStatitics>
    {
        public override void Configure(EntityTypeBuilder<SiteStatitics> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(SiteStatitics));
        }
    }
}
