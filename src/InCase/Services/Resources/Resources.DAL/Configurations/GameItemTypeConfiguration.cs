using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class GameItemTypeConfiguration : BaseEntityConfiguration<GameItemType>
    {
        private readonly List<GameItemType> types = new() {
            new() { Name = "none" }, new() { Name = "pistol" },
            new() { Name = "weapon" }, new() { Name = "rifle" },
            new() { Name = "knife" }, new() { Name = "gloves" },
            new() { Name = "other" }
        };

        public override void Configure(EntityTypeBuilder<GameItemType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemType));

            builder.HasIndex(git => git.Name)
                .IsUnique();
            builder.Property(git => git.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach (var type in types)
                builder.HasData(type);
        }
    }
}
