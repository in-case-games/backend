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

            builder.HasOne(x => x.User)
                .WithMany(t => t.Restrictions)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Owner)
                .WithMany(t => t.OwnerRestrictions)
                .HasForeignKey(m => m.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable(nameof(UserRestriction));
        }
    }
}
