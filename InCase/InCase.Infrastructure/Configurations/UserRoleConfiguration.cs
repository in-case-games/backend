using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserRole));

            builder.HasIndex(i => i.Name)
                .IsUnique();
            builder.Property(p => p.Name)
                .HasMaxLength(15)
                .IsRequired();
        }
    }
}
