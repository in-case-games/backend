using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserHistoryWithdrawConfiguration : BaseEntityConfiguration<UserHistoryWithdraw>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryWithdraw> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserHistoryWithdraw));

            builder.HasIndex(i => i.UserId)
                .IsUnique(false);
            builder.HasIndex(i => i.ItemId)
                .IsUnique(false);
            builder.HasIndex(i => i.StatusId)
                .IsUnique(false);
            builder.HasIndex(i => i.IdForMarket)
                .IsUnique();

            builder.Property(p => p.Date)
                .IsRequired();
            builder.Property(p => p.IdForMarket)
                .IsRequired();

            builder.HasOne(o => o.User)
                .WithMany(m => m.HistoryWithdrawns)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Item)
                .WithMany(m => m.HistoryWithdrawns)
                .HasForeignKey(fk => fk.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Status)
                .WithOne(o => o.HistoryWithdraw)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(o => o.Market)
                .WithMany(m => m.HistoryWithdraws)
                .HasForeignKey(fk => fk.MarketId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
