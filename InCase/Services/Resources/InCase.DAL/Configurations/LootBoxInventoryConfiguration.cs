using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class LootBoxInventoryConfiguration : BaseEntityConfiguration<LootBoxInventory>
    {
        public override void Configure(EntityTypeBuilder<LootBoxInventory> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBoxInventory));

            builder.HasIndex(i => i.BoxId)
                .IsUnique(false);
            builder.HasIndex(i => i.ItemId)
                .IsUnique(false);

            builder.Property(p => p.ChanceWining)
                .IsRequired();

            builder.HasOne(o => o.Box)
                .WithMany(m => m.Inventories)
                .HasForeignKey(o => o.BoxId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Item)
                .WithMany(m => m.Inventories)
                .HasForeignKey(o => o.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
