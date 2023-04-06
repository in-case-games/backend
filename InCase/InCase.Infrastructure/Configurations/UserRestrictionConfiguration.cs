using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
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
            builder.Property(p => p.ExpirationDate)
                .IsRequired();
            builder.Property(p => p.Description)
                .HasMaxLength(120)
                .IsRequired(false);

            builder.HasOne(x => x.User)
                .WithMany(t => t.Restrictions)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(x => x.Owner)
                .WithMany(t => t.OwnerRestrictions)
                .HasForeignKey(m => m.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
