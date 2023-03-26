using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class UserHistoryWithdrawnConfiguration : BaseEntityConfiguration<UserHistoryWithdrawn>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryWithdrawn> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserHistoryWithdrawn));
        }
    }
}
