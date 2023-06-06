using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.DAL.Configurations
{
    internal class GameItemConfiguration : BaseEntityConfiguration<GameItem>
    {
        public override void Configure(EntityTypeBuilder<GameItem> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItem));

            builder.Property(gi => gi.Cost)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();
        }
    }
}
