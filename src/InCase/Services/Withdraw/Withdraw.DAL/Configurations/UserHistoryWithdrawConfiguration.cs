using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Withdraw.DAL.Entities;

namespace Withdraw.DAL.Configurations
{
    internal class UserHistoryWithdrawConfiguration : BaseEntityConfiguration<UserHistoryWithdraw>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryWithdraw> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserHistoryWithdraw));

            builder.HasIndex(uhw => uhw.InvoiceId)
                .IsUnique(false);
            builder.HasIndex(uhw => uhw.MarketId)
                .IsUnique(false);
            builder.HasIndex(uhw => uhw.StatusId)
                .IsUnique(false);
            builder.HasIndex(uhw => uhw.UserId)
                .IsUnique(false);
            builder.HasIndex(uhw => uhw.ItemId)
                .IsUnique(false);

            builder.Property(uhw => uhw.InvoiceId)
                .IsRequired();
            builder.Property(uhw => uhw.TradeUrl)
                .IsRequired();
            builder.Property(uhw => uhw.FixedCost)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();
            builder.Property(uhw => uhw.Date)
                .IsRequired();
            builder.Property(uhw => uhw.UpdateDate)
                .IsRequired();

            builder.HasOne(uhw => uhw.Market)
                .WithMany(m => m.HistoryWithdraws)
                .HasForeignKey(uhw => uhw.MarketId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(uhw => uhw.Status)
                .WithOne(s => s.HistoryWithdraw)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(uhw => uhw.User)
                .WithMany(u => u.HistoriesWithdraws)
                .HasForeignKey(uhw => uhw.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(uhw => uhw.Item)
                .WithMany(i => i.HistoriesWithdraws)
                .HasForeignKey(uhw => uhw.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
