using CaseApplication.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class PromocodeConfiguration: BaseEntityConfiguration<Promocode>
    {
        public override void Configure(EntityTypeBuilder<Promocode> builder)
        {
            base.Configure(builder);

            builder.HasIndex(i => i.PromocodeName)
                .IsUnique();
        
            builder.Property(p => p.PromocodeDiscount)
                .HasColumnType("DECIMAL(18, 5)")
                .IsRequired();
        
            builder.HasMany(m => m.PromocodesUsedByUsers)
                .WithOne(o => o.Promocode)
                .HasForeignKey(fk => fk.PromocodeId)
                .OnDelete(DeleteBehavior.Cascade);
        
            builder.HasIndex(i => i.PromocodeTypeId)
                .IsUnique(false);
        }
    }
}