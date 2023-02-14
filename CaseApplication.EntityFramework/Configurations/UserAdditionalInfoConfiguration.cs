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

            builder.Property(p => p.UserAbleToPay)
                .HasColumnType("DECIMAL(18, 5)")
                .IsRequired();

            builder.Property(p => p.UserBalance)
                .HasColumnType("DECIMAL(18, 5)")
                .IsRequired();

            builder.HasIndex(i => i.UserRoleId)
                .IsUnique(false);

        }
    }
}
