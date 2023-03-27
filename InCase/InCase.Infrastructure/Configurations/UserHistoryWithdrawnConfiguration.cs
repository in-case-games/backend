using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities;

namespace InCase.Infrastructure.Configurations
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
