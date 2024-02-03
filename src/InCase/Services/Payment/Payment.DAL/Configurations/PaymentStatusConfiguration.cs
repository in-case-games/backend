using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.DAL.Entities;

namespace Payment.DAL.Configurations;

internal class PaymentStatusConfiguration : BaseEntityConfiguration<PaymentStatus>
{
    private readonly List<PaymentStatus> _statuses =
    [
        new PaymentStatus { Name = "wait" },
        new PaymentStatus { Name = "success" },
        new PaymentStatus { Name = "cancel" },
    ];

    public override void Configure(EntityTypeBuilder<PaymentStatus> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(PaymentStatus));

        foreach (var status in _statuses) builder.HasData(status);
    }
}
