using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promocode.DAL.Entities;

namespace Promocode.DAL.Configurations
{
    internal class UserHistoryPromocodeConfiguration : BaseEntityConfiguration<UserHistoryPromocode>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryPromocode> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserHistoryPromocode));

            builder.HasIndex(uhp => uhp.UserId)
                .IsUnique(false);
            builder.HasIndex(uhp => uhp.PromocodeId)
                .IsUnique(false);

            builder.Property(uhp => uhp.Date)
                .IsRequired();
            builder.Property(uhp => uhp.IsActivated)
                .IsRequired();

            builder.HasOne(uhp => uhp.Promocode)
                .WithMany(p => p.HistoriesPromocodes)
                .HasForeignKey(uhp => uhp.PromocodeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(uhp => uhp.User)
                .WithMany(u => u.HistoriesPromocodes)
                .HasForeignKey(uhp => uhp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
