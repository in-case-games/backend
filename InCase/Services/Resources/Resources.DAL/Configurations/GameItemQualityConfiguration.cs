using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class GameItemQualityConfiguration : BaseEntityConfiguration<GameItemQuality>
    {
        public override void Configure(EntityTypeBuilder<GameItemQuality> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemQuality));

            builder.HasIndex(giq => giq.Name)
                .IsUnique();

            builder.Property(giq => giq.Name)
                .IsRequired();
        }
    }
}
