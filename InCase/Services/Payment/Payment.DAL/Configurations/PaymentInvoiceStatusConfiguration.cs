using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.DAL.Entities;

namespace Payment.DAL.Configurations
{
    internal class PaymentInvoiceStatusConfiguration : BaseEntityConfiguration<PaymentInvoiceStatus>
    {
        public override void Configure(EntityTypeBuilder<PaymentInvoiceStatus> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(PaymentInvoiceStatus));

            builder.HasIndex(pis => pis.Name)
                .IsUnique();
            builder.Property(pis => pis.Name)
                .IsRequired();
        }
    }
}
