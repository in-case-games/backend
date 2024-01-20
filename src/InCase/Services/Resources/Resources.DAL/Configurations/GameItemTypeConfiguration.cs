using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class GameItemTypeConfiguration : BaseEntityConfiguration<GameItemType>
    {
        private readonly List<GameItemType> _types =
        [
            new GameItemType { Name = "none" }, new GameItemType { Name = "pistol" },
            new GameItemType { Name = "weapon" }, new GameItemType { Name = "rifle" },
            new GameItemType { Name = "knife" }, new GameItemType { Name = "gloves" },
            new GameItemType { Name = "other" }
        ];

        public override void Configure(EntityTypeBuilder<GameItemType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemType));

            builder.HasIndex(git => git.Name)
                .IsUnique();
            builder.Property(git => git.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach (var type in _types) builder.HasData(type);
        }
    }
}
