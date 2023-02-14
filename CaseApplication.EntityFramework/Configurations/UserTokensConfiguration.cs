using CaseApplication.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class UserTokensConfiguration : BaseEntityConfiguration<UserToken>
    {
        public override void Configure(EntityTypeBuilder<UserToken> builder)
        {
            base.Configure(builder);

            builder.HasIndex(i => i.UserId)
                .IsUnique(false);
            builder.HasIndex(i => i.RefreshToken)
                .IsUnique();
            builder.HasIndex(i => i.UserIpAddress)
                .IsUnique();
            builder.Property(p => p.UserIpAddress)
                .IsRequired();
            builder.Property(p => p.RefreshToken)
                .IsRequired();
            builder.Property(p => p.RefreshTokenExpiryTime)
                .IsRequired();
            builder.Property(p => p.RefreshTokenCreationTime)
                .IsRequired();
        }
    }
}
