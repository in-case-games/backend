using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class UserRestrictionConfiguration: BaseEntityConfiguration<UserRestriction>
    {
        public override void Configure(EntityTypeBuilder<UserRestriction> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.RestrictionName)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
