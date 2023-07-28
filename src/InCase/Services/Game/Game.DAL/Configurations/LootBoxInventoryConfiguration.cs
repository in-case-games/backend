using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.DAL.Configurations
{
    internal class LootBoxInventoryConfiguration : BaseEntityConfiguration<LootBoxInventory>
    {
        public override void Configure(EntityTypeBuilder<LootBoxInventory> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBoxInventory));

            builder.Property(lbi => lbi.ChanceWining)
                .IsRequired();

            builder.HasIndex(lbi => lbi.BoxId)
                .IsUnique(false);
            builder.HasIndex(lbi => lbi.ItemId)
                .IsUnique(false);

            builder.HasOne(lbi => lbi.Box)
                .WithMany(lb => lb.Inventories)
                .HasForeignKey(lbi => lbi.BoxId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(lbi => lbi.Item)
                .WithMany(gi => gi.Inventories)
                .HasForeignKey(lbi => lbi.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
