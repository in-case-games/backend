using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class LootBoxGroupConfiguration : BaseEntityConfiguration<LootBoxGroup>
    {
        public override void Configure(EntityTypeBuilder<LootBoxGroup> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBoxGroup));
        }
    }
}
