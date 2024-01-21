using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Identity.DAL.Entities;

namespace Identity.DAL.Configurations;

internal class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(User));

        builder.HasIndex(i => i.Login)
            .IsUnique();
        builder.Property(p => p.Login)
            .HasMaxLength(50)
            .IsRequired();
    }
}