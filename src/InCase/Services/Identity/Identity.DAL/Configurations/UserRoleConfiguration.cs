using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Identity.DAL.Entities;

namespace Identity.DAL.Configurations;
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

        builder.HasIndex(i => i.Name)
            .IsUnique();
        builder.Property(p => p.Name)
            .HasMaxLength(15)
            .IsRequired();

        foreach(var role in _roles) builder.HasData(role);
    }
}