using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.Domain.Entities;

namespace CaseApplication.Infrastructure.Configurations
{
    internal class UserInventoryConfiguration: BaseEntityConfiguration<UserInventory>
    {
        public override void Configure(EntityTypeBuilder<UserInventory> builder)
        {
            base.Configure(builder);
        }
    }
}
