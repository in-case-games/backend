using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.DAL.Configurations;
internal class UserOpeningConfiguration : BaseEntityConfiguration<UserOpening>
{
    public override void Configure(EntityTypeBuilder<UserOpening> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserOpening));

        builder.Property(uho => uho.Date)
            .IsRequired();

        builder.HasIndex(uho => uho.UserId)
            .IsUnique(false);
        builder.HasIndex(uho => uho.ItemId)
            .IsUnique(false);
        builder.HasIndex(uho => uho.BoxId)
            .IsUnique(false);

        builder.HasOne(uho => uho.User)
            .WithMany(u => u.Openings)
            .HasForeignKey(uho => uho.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(uho => uho.Item)
            .WithMany(gi => gi.Openings)
            .HasForeignKey(uho => uho.ItemId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(uho => uho.Box)
            .WithMany(lb => lb.Openings)
            .HasForeignKey(uho => uho.BoxId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}