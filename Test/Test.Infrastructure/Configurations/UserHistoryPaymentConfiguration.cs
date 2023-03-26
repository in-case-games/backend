using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class UserHistoryPaymentConfiguration : BaseEntityConfiguration<UserHistoryPayment>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryPayment> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserHistoryPayment));
        }
    }
}
