using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class GameItemQualityConfiguration : BaseEntityConfiguration<GameItemQuality>
    {
        private readonly List<GameItemQuality> _qualities =
        [
            new GameItemQuality { Name = "none" }, new GameItemQuality { Name = "battle scarred" },
            new GameItemQuality { Name = "well worn" }, new GameItemQuality { Name = "field tested" },
            new GameItemQuality { Name = "minimal wear" }, new GameItemQuality { Name = "factory new" }
        ];

        public override void Configure(EntityTypeBuilder<GameItemQuality> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemQuality));

            builder.HasIndex(giq => giq.Name)
                .IsUnique();

            builder.Property(giq => giq.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach (var quality in _qualities) builder.HasData(quality);
        }
    }
}
