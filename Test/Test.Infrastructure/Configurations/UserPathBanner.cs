using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class UserPathBannerConfiguration : BaseEntityConfiguration<UserPathBanner>
    {
        public override void Configure(EntityTypeBuilder<UserPathBanner> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserPathBanner));
        }
    }
}
