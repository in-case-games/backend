using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class GameItemTypeConfiguration : BaseEntityConfiguration<GameItemType>
    {
        public override void Configure(EntityTypeBuilder<GameItemType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemType));

            builder.HasIndex(git => git.Name)
                .IsUnique();
            builder.Property(git => git.Name)
                .IsRequired();
        }
    }
}
