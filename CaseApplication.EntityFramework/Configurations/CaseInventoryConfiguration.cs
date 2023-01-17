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

            builder.HasOne(o => o.GameCase)
                .WithMany(m => m.CaseInventories)
                .HasForeignKey(fk => fk.GameCaseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.CaseItem)
                .WithMany(m => m.CaseInventories)
                .HasForeignKey(fk => fk.CaseItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Property(p => p.LossChance)
                .HasColumnType("DECIMAL(6, 5)")
                .IsRequired();

        }
    }
}
