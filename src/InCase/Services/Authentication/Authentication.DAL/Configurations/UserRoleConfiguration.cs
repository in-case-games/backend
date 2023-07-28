using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.DAL.Configurations
{
    internal class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
    {
        private readonly List<UserRole> roles = new() {
            new() { Name = "user" }, new() { Name = "admin" },
            new() { Name = "owner" }, new() { Name = "bot" },
        };

        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserRole));

            builder.Property(ur => ur.Name)
                .HasMaxLength(15)
                .IsRequired();
            builder.HasIndex(ur => ur.Name)
                .IsUnique();

            foreach (var role in roles)
                builder.HasData(role);
        }
    }
}
