using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class UserInventoryConfiguration : BaseEntityConfiguration<UserInventory>
    {
        public override void Configure(EntityTypeBuilder<UserInventory> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserInventory));
        }
    }
}
