using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class SiteStatiticsAdminConfiguration : BaseEntityConfiguration<SiteStatiticsAdmin>
    {
        public override void Configure(EntityTypeBuilder<SiteStatiticsAdmin> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(SiteStatiticsAdmin));
        }
    }
}
