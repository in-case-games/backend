using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promocode.DAL.Entities;

namespace Promocode.DAL.Configurations;
internal class UserHistoryPromoCodeConfiguration : BaseEntityConfiguration<UserPromoCode>
{
    public override void Configure(EntityTypeBuilder<UserPromoCode> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserPromoCode));

        builder.HasIndex(uhp => uhp.UserId)
            .IsUnique(false);
        builder.HasIndex(uhp => uhp.PromoCodeId)
            .IsUnique(false);

        builder.Property(uhp => uhp.Date)
            .IsRequired();
        builder.Property(uhp => uhp.IsActivated)
            .IsRequired();

        builder.HasOne(uhp => uhp.PromoCode)
            .WithMany(p => p.HistoriesPromoCodes)
            .HasForeignKey(uhp => uhp.PromoCodeId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(uhp => uhp.User)
            .WithMany(u => u.HistoriesPromoCodes)
            .HasForeignKey(uhp => uhp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}