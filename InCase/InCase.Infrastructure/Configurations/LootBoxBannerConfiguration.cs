using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class LootBoxBannerConfiguration : BaseEntityConfiguration<LootBoxBanner>
    {
        public override void Configure(EntityTypeBuilder<LootBoxBanner> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBoxBanner));
        }
    }
}
