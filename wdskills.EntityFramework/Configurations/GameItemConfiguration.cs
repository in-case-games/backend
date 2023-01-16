using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wdskills.DomainLayer.Entities;

namespace wdskills.EntityFramework.Configurations
{
    internal class GameItemConfiguration: BaseEntityConfiguration<GameItem>
    {
        public override void Configure(EntityTypeBuilder<GameItem> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.GameItemName)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(i => i.GameItemName)
                .IsUnique();

            builder.Property(p => p.GameItemCost)
                .HasPrecision(18, 5)
                .IsRequired();

            builder.Property(p => p.GameItemImage)
                .IsRequired();

            builder.Property(p => p.GameItemRarity)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
