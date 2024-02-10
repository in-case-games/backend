using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promocode.DAL.Entities;

namespace Promocode.DAL.Configurations;
internal class PromoCodeEntityConfiguration : BaseEntityConfiguration<PromoCode>
{
    public override void Configure(EntityTypeBuilder<PromoCode> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(PromoCode));

        builder.HasIndex(pe => pe.Name)
            .IsUnique();
        builder.HasIndex(pe => pe.TypeId)
            .IsUnique(false);

        builder.Property(pe => pe.Name)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(p => p.Discount)
            .HasColumnType("DECIMAL(5,5)")
            .IsRequired();
        builder.Property(pe => pe.NumberActivations)
            .IsRequired();
        builder.Property(pe => pe.ExpirationDate)
            .IsRequired();

        builder.HasOne(pe => pe.Type)
            .WithOne(pt => pt.PromoCode)
            .OnDelete(DeleteBehavior.Cascade);
    }
}