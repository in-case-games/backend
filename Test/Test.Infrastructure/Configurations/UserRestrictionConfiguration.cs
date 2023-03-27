using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class UserRestrictionConfiguration : BaseEntityConfiguration<UserRestriction>
    {
        public override void Configure(EntityTypeBuilder<UserRestriction> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserRestriction));
        }
    }
}
