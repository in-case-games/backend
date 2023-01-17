using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class UserInventoryConfiguration: BaseEntityConfiguration<UserInventory>
    {
        public override void Configure(EntityTypeBuilder<UserInventory> builder)
        {
            base.Configure(builder);

            builder.HasOne(o => o.User)
                .WithMany(m => m.UserInventories)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.GameItem)
                .WithMany(m => m.UserInventories)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
