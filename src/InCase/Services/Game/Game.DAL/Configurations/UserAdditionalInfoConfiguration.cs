using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.DAL.Configurations;

internal class UserAdditionalInfoConfiguration : BaseEntityConfiguration<UserAdditionalInfo>
{
    public override void Configure(EntityTypeBuilder<UserAdditionalInfo> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserAdditionalInfo));

        builder.HasIndex(uai => uai.UserId)
            .IsUnique();

        builder.Property(uai => uai.IsGuestMode)
            .IsRequired();

        builder.Property(uai => uai.Balance)
            .HasColumnType("DECIMAL(18,5)")
            .IsRequired();

        builder.HasOne(uai => uai.User)
            .WithOne(u => u.AdditionalInfo)
            .OnDelete(DeleteBehavior.Cascade);
    }
}