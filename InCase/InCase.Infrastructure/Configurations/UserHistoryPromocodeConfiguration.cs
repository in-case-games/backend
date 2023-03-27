using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserHistoryPromocodeConfiguration : BaseEntityConfiguration<UserHistoryPromocode>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryPromocode> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserHistoryPromocode));
        }
    }
}
