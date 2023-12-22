using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class GameItemRarityConfiguration : BaseEntityConfiguration<GameItemRarity>
    {
        private readonly List<GameItemRarity> _rarities = new() {
            new GameItemRarity { Name = "white" }, new GameItemRarity { Name = "blue" },
            new GameItemRarity { Name = "violet" }, new GameItemRarity { Name = "pink" },
            new GameItemRarity { Name = "red" }, new GameItemRarity { Name = "gold" }
        };

        public override void Configure(EntityTypeBuilder<GameItemRarity> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemRarity));

            builder.HasIndex(gir => gir.Name)
                .IsUnique();

            builder.Property(gir => gir.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach (var rarity in _rarities) builder.HasData(rarity);
        }
    }
}
