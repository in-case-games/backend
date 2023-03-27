using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class UserHistoryOpeningConfiguration : BaseEntityConfiguration<UserHistoryOpening>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryOpening> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserHistoryOpening));
        }
    }
}
