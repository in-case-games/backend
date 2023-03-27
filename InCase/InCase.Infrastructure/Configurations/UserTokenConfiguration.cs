using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserTokenConfiguration : BaseEntityConfiguration<UserToken>
    {
        public override void Configure(EntityTypeBuilder<UserToken> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserToken));
        }
    }
}
