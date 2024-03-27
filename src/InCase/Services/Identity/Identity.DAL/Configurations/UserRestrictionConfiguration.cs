using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Identity.DAL.Entities;

namespace Identity.DAL.Configurations;
internal class UserRestrictionConfiguration : BaseEntityConfiguration<UserRestriction>
{
    public override void Configure(EntityTypeBuilder<UserRestriction> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(UserRestriction));

        builder.HasIndex(i => i.UserId)
            .IsUnique(false);
        builder.HasIndex(i => i.OwnerId)
            .IsUnique(false);
        builder.HasIndex(i => i.TypeId)
            .IsUnique(false);

        builder.Property(p => p.CreationDate)
            .IsRequired();
        builder.Property(p => p.Description)
            .HasMaxLength(120)
            .IsRequired(false);

        builder.HasOne(x => x.User)
            .WithMany(t => t.Restrictions)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ur => ur.Owner)
            .WithMany(u => u.OwnerRestrictions)
            .HasForeignKey(ur => ur.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}