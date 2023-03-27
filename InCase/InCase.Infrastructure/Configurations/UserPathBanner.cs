using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities;

namespace InCase.Infrastructure.Configurations
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
