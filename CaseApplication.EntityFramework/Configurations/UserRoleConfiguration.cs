using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class UserRoleConfiguration: BaseEntityConfiguration<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.RoleName)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasOne(o => o.UserAdditionalInfo)
                .WithOne(o => o.UserRole);
        }
    }
}
