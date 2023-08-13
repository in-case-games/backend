using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class GameItemConfiguration : BaseEntityConfiguration<GameItem>
    {
        public override void Configure(EntityTypeBuilder<GameItem> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItem));

            builder.HasIndex(gi => gi.GameId)
                .IsUnique(false);
            builder.HasIndex(gi => gi.TypeId)
                .IsUnique(false);
            builder.HasIndex(gi => gi.QualityId)
                .IsUnique(false);
            builder.HasIndex(gi => gi.RarityId)
                .IsUnique(false);

            builder.Property(gi => gi.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(gi => gi.UpdateDate)
                .IsRequired();
            builder.Property(gi => gi.HashName)
                .IsRequired(false);
            builder.Property(gi => gi.IdForMarket)
                .IsRequired();
            builder.Property(gi => gi.Cost)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();

            builder.HasOne(gi => gi.Game)
                .WithMany(g => g.Items)
                .HasForeignKey(gi => gi.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(gi => gi.Rarity)
                .WithOne(gir => gir.Item)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(gi => gi.Quality)
                .WithOne(giq => giq.Item)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(gi => gi.Type)
                .WithOne(git => git.Item)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
