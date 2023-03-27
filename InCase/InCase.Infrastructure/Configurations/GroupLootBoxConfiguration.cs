using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class GroupLootBoxConfiguration : BaseEntityConfiguration<GroupLootBox>
    {
        public override void Configure(EntityTypeBuilder<GroupLootBox> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GroupLootBox));
        }
    }
}
