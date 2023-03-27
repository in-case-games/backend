using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class LootBoxConfiguration : BaseEntityConfiguration<LootBox>
    {
        public override void Configure(EntityTypeBuilder<LootBox> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBox));
        }
    }
}
