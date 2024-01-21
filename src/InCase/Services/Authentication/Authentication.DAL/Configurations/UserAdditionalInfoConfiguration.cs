using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.DAL.Configurations;

internal class UserAdditionalInfoConfiguration : BaseEntityConfiguration<UserAdditionalInfo>
{
    public override void Configure(EntityTypeBuilder<UserAdditionalInfo> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserAdditionalInfo));

        builder.Property(uai => uai.IsConfirmed)
            .IsRequired();
        builder.Property(uai => uai.DeletionDate)
            .IsRequired(false);

        builder.HasIndex(uai => uai.UserId)
            .IsUnique();
        builder.HasIndex(uai => uai.RoleId)
            .IsUnique(false);

        builder.HasOne(uai => uai.User)
            .WithOne(u => u.AdditionalInfo)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(uai => uai.Role)
            .WithOne(ur => ur.AdditionalInfo)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
