using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wdskills.DomainLayer.Entities;

namespace wdskills.EntityFramework.Configurations
{
    internal class UserRoleConfiguration: BaseEntityConfiguration<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.RoleName)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
