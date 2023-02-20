using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.Domain.Entities.Internal;

namespace CaseApplication.Infrastructure.Configurations
{
    internal class CaseInventoryConfiguration: BaseEntityConfiguration<CaseInventory>
    {
        public override void Configure(EntityTypeBuilder<CaseInventory> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.LossChance)
                .IsRequired();

            builder.HasIndex(i => i.GameCaseId)
                .IsUnique(false);
            builder.HasIndex(i => i.GameItemId)
                .IsUnique(false);
        }
    }
}
