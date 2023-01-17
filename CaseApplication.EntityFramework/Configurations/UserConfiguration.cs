using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.DomainLayer.Entities;

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

        }
    }
}
