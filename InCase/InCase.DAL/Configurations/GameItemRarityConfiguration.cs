using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class GameItemRarityConfiguration : BaseEntityConfiguration<GameItemRarity>
    {
        private readonly List<GameItemRarity> rarities = new() {
            new() { Name = "white" }, new() { Name = "blue" },
            new() { Name = "violet" }, new() { Name = "pink" },
            new() { Name = "red" }, new() { Name = "gold" }
        };
        public override void Configure(EntityTypeBuilder<GameItemRarity> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemRarity));
            
            builder.HasIndex(i => i.Name)
                .IsUnique();
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach(var rarity in rarities)
                builder.HasData(rarity);
        }
    }
}
