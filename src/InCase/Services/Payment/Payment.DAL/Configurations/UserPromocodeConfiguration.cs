using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.DAL.Entities;

namespace Payment.DAL.Configurations;
internal class UserPromoCodeConfiguration : BaseEntityConfiguration<UserPromoCode>
{
    public override void Configure(EntityTypeBuilder<UserPromoCode> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserPromoCode));

        builder.Property(up => up.Discount)
            .HasColumnType("DECIMAL(5,5)")
            .IsRequired();

        builder.HasOne(up => up.User)
            .WithOne(u => u.PromoCode)
            .OnDelete(DeleteBehavior.Cascade);
    }
}