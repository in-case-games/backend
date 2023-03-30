using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class GameItemTypeConfiguration : BaseEntityConfiguration<GameItemType>
    {
        public override void Configure(EntityTypeBuilder<GameItemType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemType));
            
            builder.HasIndex(i => i.Name)
                .IsUnique();
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
