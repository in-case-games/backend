using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.DAL.Entities;

namespace Payment.DAL.Configurations;

internal class UserPromocodeConfiguration : BaseEntityConfiguration<UserPromocode>
{
    public override void Configure(EntityTypeBuilder<UserPromocode> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserPromocode));

        builder.Property(up => up.Discount)
            .HasColumnType("DECIMAL(5,5)")
            .IsRequired();

        builder.HasOne(up => up.User)
            .WithOne(u => u.Promocode)
            .OnDelete(DeleteBehavior.Cascade);
    }
}