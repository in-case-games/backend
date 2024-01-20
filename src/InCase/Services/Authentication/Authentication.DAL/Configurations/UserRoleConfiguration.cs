using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.DAL.Configurations
{
    internal class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
    {
        private readonly List<UserRole> _roles =
        [
            new UserRole { Name = "user" }, new UserRole { Name = "admin" },
            new UserRole { Name = "owner" }, new UserRole { Name = "bot" }
        ];

        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserRole));

            builder.Property(ur => ur.Name)
                .HasMaxLength(15)
                .IsRequired();
            builder.HasIndex(ur => ur.Name)
                .IsUnique();

            foreach (var role in _roles) builder.HasData(role);
        }
    }
}
