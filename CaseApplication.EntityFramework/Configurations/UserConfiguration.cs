using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class UserConfiguration: BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.UserName)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(p => p.UserEmail)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(i => i.UserEmail)
                .IsUnique();

            builder.HasOne(o => o.UserAdditionalInfo)
                .WithOne(o => o.User)
                .HasForeignKey<UserAdditionalInfo>(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.UserRestrictions)
                .WithOne(o => o.User)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.UserInventories)
                .WithOne(o => o.User)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
