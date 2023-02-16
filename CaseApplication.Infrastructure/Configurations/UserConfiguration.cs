using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.Infrastructure.Configurations
{
    internal class UserConfiguration: BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.HasIndex(i => i.UserEmail).IsUnique();
            builder.HasIndex(i => i.UserLogin).IsUnique();
            builder.HasIndex(i => i.PasswordSalt).IsUnique();

            builder.Property(p => p.PasswordHash).IsRequired();
            builder.Property(p => p.PasswordSalt).IsRequired();

            builder.Property(p => p.UserLogin)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(p => p.UserEmail)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(o => o.UserAdditionalInfo)
                .WithOne(o => o.User)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(m => m.UserRestrictions)
                .WithOne(o => o.User)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.UserInventories)
                .WithOne(o => o.User)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.UserHistoryOpeningCases)
                .WithOne(o => o.User)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.PromocodesUsedByUsers)
                .WithOne(o => o.User)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.UserTokens)
                .WithOne(o => o.User)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
