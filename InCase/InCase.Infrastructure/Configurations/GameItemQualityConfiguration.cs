using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class GameItemQualityConfiguration : BaseEntityConfiguration<GameItemQuality>
    {
        private readonly List<GameItemQuality> qualities = new() {
            new() { Name = "none" }, new() { Name = "battle scarred" },
            new() { Name = "well worn" }, new() { Name = "field tested" },
            new() { Name = "minimal wear" }, new() { Name = "factory new" },
        };

        public override void Configure(EntityTypeBuilder<GameItemQuality> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemQuality));
            
            builder.HasIndex(i => i.Name)
                .IsUnique();
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach(var quality in qualities) 
                builder.HasData(quality);
        }
    }
}
