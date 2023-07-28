using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Withdraw.DAL.Entities;

namespace Withdraw.DAL.Configurations
{
    internal class GameItemConfiguration : BaseEntityConfiguration<GameItem>
    {
        public override void Configure(EntityTypeBuilder<GameItem> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItem));

            builder.HasIndex(gi => gi.GameId)
                .IsUnique(false);

            builder.Property(gi => gi.IdForMarket)
                .IsRequired();
            builder.Property(gi => gi.Cost)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();

            builder.HasOne(gi => gi.Game)
                .WithMany(g => g.Items)
                .HasForeignKey(gi => gi.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
