using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class GameItemRarityConfiguration : BaseEntityConfiguration<GameItemRarity>
    {
        public override void Configure(EntityTypeBuilder<GameItemRarity> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemRarity));

            builder.HasIndex(gir => gir.Name)
                .IsUnique();

            builder.Property(gir => gir.Name)
                .IsRequired();
        }
    }
}
