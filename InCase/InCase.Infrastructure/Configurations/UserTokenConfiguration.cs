using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserTokenConfiguration : BaseEntityConfiguration<UserToken>
    {
        public override void Configure(EntityTypeBuilder<UserToken> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserToken));

            builder.Property(p => p.Refresh)
                .IsRequired();
            builder.Property(p => p.Email)
                .IsRequired();
            builder.Property(p => p.IpAddress)
                .IsRequired(false);
            builder.Property(p => p.Device)
                .IsRequired(false);

            builder.HasIndex(i => i.UserId)
                .IsUnique(false);

            builder.HasOne(o => o.User)
                .WithMany(m => m.Tokens)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
