using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promocode.DAL.Entities;

namespace Promocode.DAL.Configurations;
internal class PromoCodeTypeConfiguration : BaseEntityConfiguration<PromoCodeType>
{
    private readonly List<PromoCodeType> _types = [
        new PromoCodeType { Name = "balance" }, 
        new PromoCodeType { Name = "box" }
    ];

    public override void Configure(EntityTypeBuilder<PromoCodeType> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(PromoCodeType));

        builder.Property(pt => pt.Name)
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(pt => pt.Name)
            .IsUnique();

        foreach(var type in _types) builder.HasData(type);
    }
}