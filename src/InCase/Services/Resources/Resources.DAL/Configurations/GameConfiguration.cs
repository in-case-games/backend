using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations;
internal class GameConfiguration : BaseEntityConfiguration<Game>
{
    public override void Configure(EntityTypeBuilder<Game> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(Game));

        builder.HasIndex(g => g.Name)
            .IsUnique();
        builder.Property(g => g.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}