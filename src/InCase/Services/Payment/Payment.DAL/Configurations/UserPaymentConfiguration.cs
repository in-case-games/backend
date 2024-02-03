using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.DAL.Entities;

namespace Payment.DAL.Configurations;

internal class UserPaymentConfiguration : BaseEntityConfiguration<UserPayment>
{
    public override void Configure(EntityTypeBuilder<UserPayment> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserPayment));

        builder.HasIndex(up => up.UserId)
            .IsUnique(false);
        builder.HasIndex(up => up.StatusId)
            .IsUnique(false);

        builder.Property(up => up.Date)
            .IsRequired();
        builder.Property(up => up.Currency)
            .IsRequired();
        builder.Property(up => up.Amount)
            .HasColumnType("DECIMAL(18,5)")
            .IsRequired();

        builder.HasOne(up => up.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(up => up.Status)
            .WithOne(p => p.Payment)
            .OnDelete(DeleteBehavior.Cascade);
    }
}