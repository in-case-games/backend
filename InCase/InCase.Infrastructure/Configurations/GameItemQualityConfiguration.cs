using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class GameItemQualityConfiguration : BaseEntityConfiguration<GameItemQuality>
    {
        public override void Configure(EntityTypeBuilder<GameItemQuality> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemQuality));
        }
    }
}
