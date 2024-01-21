using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.DAL.Configurations;

internal class UserRestrictionConfiguration : BaseEntityConfiguration<UserRestriction>
{
    public override void Configure(EntityTypeBuilder<UserRestriction> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserRestriction));

        builder.Property(ur => ur.ExpirationDate)
            .IsRequired();
        builder.HasIndex(ur => ur.UserId)
            .IsUnique(false);

        builder.HasOne(ur => ur.User)
            .WithOne(u => u.Restriction)
            .OnDelete(DeleteBehavior.Cascade);
    }
}