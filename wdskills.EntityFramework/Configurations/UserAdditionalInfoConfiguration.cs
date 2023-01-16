using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using wdskills.DomainLayer.Entities;

namespace wdskills.EntityFramework.Configurations
{
    internal class UserAdditionalInfoConfiguration: BaseEntityConfiguration<UserAdditionalInfo>
    {
        public override void Configure(EntityTypeBuilder<UserAdditionalInfo> builder)
        {
            base.Configure(builder);

            builder.HasOne(o => o.User)
                .WithMany(m => m.UserAdditionalInfos)
                .HasForeignKey(fk => fk.User)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.UserAbleToPay)
                .HasDefaultValue(0);

            builder.HasOne(i => i.UserRole)
                .WithMany(m => m.UserAdditionalInfos)
                .HasForeignKey(fk => fk.UserRole)
                .IsRequired();
        }
    }
}
