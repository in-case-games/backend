using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserHistoryWithdrawnConfiguration : BaseEntityConfiguration<UserHistoryWithdrawn>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryWithdrawn> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserHistoryWithdrawn));

            builder.HasIndex(i => i.UserId)
                .IsUnique(false);
            builder.HasIndex(i => i.ItemId)
                .IsUnique(false);

            builder.Property(p => p.Date)
                .IsRequired();

            builder.HasOne(o => o.User)
                .WithMany(m => m.HistoryWithdrawns)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(o => o.Item)
                .WithMany(m => m.HistoryWithdrawns)
                .HasForeignKey(o => o.ItemId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
