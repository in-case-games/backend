using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.DAL.Entities;

namespace Payment.DAL.Configurations;
internal class PaymentStatusConfiguration : BaseEntityConfiguration<PaymentStatus>
{
    private readonly List<PaymentStatus> _statuses =
    [
        new PaymentStatus { Name = "pending" },
        new PaymentStatus { Name = "waiting" },
        new PaymentStatus { Name = "succeeded" },
        new PaymentStatus { Name = "canceled" },
    ];

    public override void Configure(EntityTypeBuilder<PaymentStatus> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(PaymentStatus));

        builder.HasIndex(ps => ps.Name)
            .IsUnique();
        builder.Property(ps => ps.Name)
            .HasMaxLength(50)
            .IsRequired();

        foreach (var status in _statuses) builder.HasData(status);
    }
}
