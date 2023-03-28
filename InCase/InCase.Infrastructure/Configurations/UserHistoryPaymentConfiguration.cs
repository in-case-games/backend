using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserHistoryPaymentConfiguration : BaseEntityConfiguration<UserHistoryPayment>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryPayment> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserHistoryPayment));

            builder.HasIndex(x => x.UserId)
                .IsUnique(false);

            builder.Property(p => p.Date)
                .IsRequired();
            builder.Property(p => p.Amount)
                .IsRequired();

            builder.HasOne(o => o.User)
                .WithMany(m => m.HistoryPayments)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
