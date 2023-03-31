using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserAdditionalInfoConfiguration : BaseEntityConfiguration<UserAdditionalInfo>
    {
        public override void Configure(EntityTypeBuilder<UserAdditionalInfo> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserAdditionalInfo));
            
            builder.HasIndex(i => i.RoleId)
                .IsUnique(false);

            builder.Property(p => p.Balance)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();
            builder.Property(p => p.ImageUri)
                .IsRequired();
            builder.Property(p => p.IsNotifyEmail)
                .IsRequired();
            builder.Property(p => p.IsGuestMode)
                .IsRequired();
            builder.Property(p => p.IsConfirmed)
                .IsRequired();
            builder.Property(p => p.CreationDate)
                .IsRequired();

            builder.HasOne(o => o.User)
                .WithOne(o => o.AdditionalInfo)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Role)
                .WithOne(o => o.AdditionalInfo)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
