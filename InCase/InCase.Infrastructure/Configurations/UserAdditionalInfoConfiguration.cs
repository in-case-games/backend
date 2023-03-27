using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserAdditionalInfoConfiguration : BaseEntityConfiguration<UserAdditionalInfo>
    {
        public override void Configure(EntityTypeBuilder<UserAdditionalInfo> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserAdditionalInfo));
        }
    }
}
