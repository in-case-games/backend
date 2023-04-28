using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class GameMarketConfiguration : BaseEntityConfiguration<GameMarket>
    {
        public override void Configure(EntityTypeBuilder<GameMarket> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameMarket));

            builder.HasIndex(i => i.GameId)
                .IsUnique(false);

            builder.HasIndex(i => i.Name)
                .IsUnique(false);

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(o => o.Game)
                .WithMany(o => o.Markets)
                .HasForeignKey(fk => fk.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
