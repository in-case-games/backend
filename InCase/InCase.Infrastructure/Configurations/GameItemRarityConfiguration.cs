using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class GameItemRarityConfiguration : BaseEntityConfiguration<GameItemRarity>
    {
        public override void Configure(EntityTypeBuilder<GameItemRarity> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemRarity));
        }
    }
}
