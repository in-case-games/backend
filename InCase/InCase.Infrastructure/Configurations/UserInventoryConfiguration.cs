using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserInventoryConfiguration : BaseEntityConfiguration<UserInventory>
    {
        public override void Configure(EntityTypeBuilder<UserInventory> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserInventory));

            builder.HasIndex(i => i.UserId)
                .IsUnique(false);
            builder.HasIndex(i => i.ItemId)
                .IsUnique(false);

            builder.Property(p => p.Date)
                .IsRequired();
            builder.Property(p => p.FixedCost)
                .IsRequired();

            builder.HasOne(o => o.User)
                .WithMany(m => m.Inventories)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Item)
                .WithMany(m => m.UserInventories)
                .HasForeignKey(o => o.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
