using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace InCase.Infrastructure.Configurations
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

            builder.HasIndex(i => i.Name)
                .IsUnique();
            builder.Property(p => p.Name)
                .HasMaxLength(15)
                .IsRequired();

            foreach(var role in roles) 
                builder.HasData(role);
        }
    }
}
