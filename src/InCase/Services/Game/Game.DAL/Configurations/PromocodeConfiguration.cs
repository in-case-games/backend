using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.DAL.Configurations;
internal class PromoCodeConfiguration : BaseEntityConfiguration<UserPromoCode>
{
    public override void Configure(EntityTypeBuilder<UserPromoCode> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserPromoCode));

        builder.Property(gi => gi.Discount)
            .HasColumnType("DECIMAL(18,5)")
            .IsRequired();
    }
}