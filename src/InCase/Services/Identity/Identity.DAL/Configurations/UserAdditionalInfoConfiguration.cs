using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Identity.DAL.Entities;

namespace Identity.DAL.Configurations
{
    internal class UserAdditionalInfoConfiguration : BaseEntityConfiguration<UserAdditionalInfo>
    {
        public override void Configure(EntityTypeBuilder<UserAdditionalInfo> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserAdditionalInfo));
            
            builder.HasIndex(i => i.RoleId)
                .IsUnique(false);

            builder.HasOne(o => o.User)
                .WithOne(o => o.AdditionalInfo)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Role)
                .WithOne(o => o.AdditionalInfo)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
