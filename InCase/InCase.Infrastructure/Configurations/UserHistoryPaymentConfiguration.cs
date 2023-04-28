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

            builder.HasIndex(i => i.UserId)
                .IsUnique(false);
            builder.HasIndex(i => i.StatusId)
                .IsUnique(false);

            builder.Property(p => p.Date)
                .IsRequired();
            builder.Property(p => p.Currency)
                .IsRequired();
            builder.Property(p => p.Rate)
                .HasColumnType("DECIMAL(6,5)")
                .IsRequired();
            builder.Property(p => p.Amount)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();

            builder.HasOne(o => o.User)
                .WithMany(m => m.HistoryPayments)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Status)
                .WithOne(o => o.HistoryPayment)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
