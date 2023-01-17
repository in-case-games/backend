using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class UserAdditionalInfoConfiguration: BaseEntityConfiguration<UserAdditionalInfo>
    {
        public override void Configure(EntityTypeBuilder<UserAdditionalInfo> builder)
        {
            base.Configure(builder);

            builder.HasOne(o => o.User)
                .WithMany(m => m.UserAdditionalInfos)
                .HasForeignKey(fk => fk.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.UserAbleToPay)
                .HasColumnType("DECIMAL(6, 5)")
                .IsRequired();

            builder.Property(p => p.UserBalance)
                .HasColumnType("DECIMAL(6, 5)")
                .IsRequired();

            builder.HasOne(i => i.UserRole)
                .WithMany(m => m.UserAdditionalInfos)
                .HasForeignKey(fk => fk.UserRoleId)
                .IsRequired();
        }
    }
}
