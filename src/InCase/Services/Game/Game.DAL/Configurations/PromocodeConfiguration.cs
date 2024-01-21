using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.DAL.Configurations;

internal class PromocodeConfiguration : BaseEntityConfiguration<UserPromocode>
{
    public override void Configure(EntityTypeBuilder<UserPromocode> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserPromocode));

        builder.Property(gi => gi.Discount)
            .HasColumnType("DECIMAL(18,5)")
            .IsRequired();
    }
}