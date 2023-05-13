using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class InvoicePaymentStatusConfiguration : BaseEntityConfiguration<InvoicePaymentStatus>
    {
        private readonly List<InvoicePaymentStatus> statuses = new() {
            new() { Name = "new" }, new() { Name = "processing" },
            new() { Name = "paid" }, new() { Name = "chargeback" },
            new() { Name = "refund" }, new() { Name = "chargeback-cancel" },
            new() { Name = "refused" }
        };

        public override void Configure(EntityTypeBuilder<InvoicePaymentStatus> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(InvoicePaymentStatus));

            builder.HasIndex(i => i.Name)
                .IsUnique();
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach (var status in statuses)
                builder.HasData(status);
        }
    }
}
