using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Withdraw.DAL.Entities;

namespace Withdraw.DAL.Configurations;

internal class GameConfiguration : BaseEntityConfiguration<Game>
{
    public override void Configure(EntityTypeBuilder<Game> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(Game));

        builder.HasIndex(g => g.Name)
            .IsUnique();

        builder.Property(g => g.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}