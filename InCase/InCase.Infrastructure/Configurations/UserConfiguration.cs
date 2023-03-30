using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(User));

            builder.HasIndex(i => i.Login)
                .IsUnique();
            builder.HasIndex(i => i.Email)
                .IsUnique();

            builder.Property(p => p.Login)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.PasswordSalt)
                .HasMaxLength(64)
                .IsRequired();
            builder.Property(p => p.PasswordHash)
                .HasMaxLength(64)
                .IsRequired();
        }
    }
}
