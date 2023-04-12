using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class GameItemTypeConfiguration : BaseEntityConfiguration<GameItemType>
    {
        private readonly List<GameItemType> types = new() {
            new() { Name = "none" }, new() { Name = "pistol" },
            new() { Name = "weapon" }, new() { Name = "rifle" },
            new() { Name = "knife" }, new() { Name = "gloves" }
        };

        public override void Configure(EntityTypeBuilder<GameItemType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemType));
            
            builder.HasIndex(i => i.Name)
                .IsUnique();
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach(var type in types)
                builder.HasData(type);
        }
    }
}
