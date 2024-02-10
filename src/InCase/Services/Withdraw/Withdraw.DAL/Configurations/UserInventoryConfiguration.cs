using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Withdraw.DAL.Entities;

namespace Withdraw.DAL.Configurations;
internal class UserInventoryConfiguration : BaseEntityConfiguration<UserInventory>
{
    public override void Configure(EntityTypeBuilder<UserInventory> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserInventory));

        builder.HasIndex(ui => ui.ItemId)
            .IsUnique(false);
        builder.HasIndex(ui => ui.UserId)
            .IsUnique(false);

        builder.Property(ui => ui.Date)
            .IsRequired();
        builder.Property(ui => ui.FixedCost)
            .HasColumnType("DECIMAL(18,5)")
            .IsRequired();

        builder.HasOne(ui => ui.User)
            .WithMany(u => u.Inventories)
            .HasForeignKey(ui => ui.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ui => ui.Item)
            .WithMany(i => i.Inventories)
            .HasForeignKey(ui => ui.ItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}