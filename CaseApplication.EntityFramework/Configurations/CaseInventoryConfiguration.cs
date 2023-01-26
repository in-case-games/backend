using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class CaseInventoryConfiguration: BaseEntityConfiguration<CaseInventory>
    {
        public override void Configure(EntityTypeBuilder<CaseInventory> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.LossChance)
                .HasColumnType("DECIMAL(18, 5)")
                .IsRequired();

            builder.HasIndex(k => k.GameCaseId)
                .IsUnique(false);
            builder.HasIndex(k => k.GameItemId)
                .IsUnique(false);
        }
    }
}
