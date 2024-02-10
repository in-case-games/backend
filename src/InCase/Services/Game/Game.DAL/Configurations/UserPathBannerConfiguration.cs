using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.DAL.Configurations;
internal class UserPathBannerConfiguration : BaseEntityConfiguration<UserPathBanner>
{
    public override void Configure(EntityTypeBuilder<UserPathBanner> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserPathBanner));

        builder.Property(upb => upb.NumberSteps)
            .IsRequired();

        builder.Property(upb => upb.FixedCost)
            .HasColumnType("DECIMAL(18,5)")
            .IsRequired();

        builder.HasIndex(upb => upb.UserId)
            .IsUnique(false);
        builder.HasIndex(upb => upb.ItemId)
            .IsUnique(false);
        builder.HasIndex(upb => upb.BoxId)
            .IsUnique(false);

        builder.HasOne(upb => upb.User)
            .WithMany(u => u.Paths)
            .HasForeignKey(upb => upb.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(upb => upb.Item)
            .WithMany(gi => gi.Paths)
            .HasForeignKey(upb => upb.ItemId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(upb => upb.Box)
            .WithMany(lb => lb.Paths)
            .HasForeignKey(upb => upb.BoxId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}