using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
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
