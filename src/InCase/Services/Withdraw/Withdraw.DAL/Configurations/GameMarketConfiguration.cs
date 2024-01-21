using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Withdraw.DAL.Entities;

namespace Withdraw.DAL.Configurations;

internal class GameMarketConfiguration : BaseEntityConfiguration<GameMarket>
{
    public override void Configure(EntityTypeBuilder<GameMarket> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(GameMarket));

        builder.HasIndex(gm => gm.GameId)
            .IsUnique(false);
        builder.HasIndex(gm => gm.Name)
            .IsUnique(false);

        builder.Property(gm => gm.Name)
            .IsRequired();

        builder.HasOne(gm => gm.Game)
            .WithOne(g => g.Market)
            .OnDelete(DeleteBehavior.Cascade);
    }
}