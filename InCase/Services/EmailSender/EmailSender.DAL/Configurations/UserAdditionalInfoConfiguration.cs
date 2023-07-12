using EmailSender.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailSender.DAL.Configurations
{
    internal class UserAdditionalInfoConfiguration : BaseEntityConfiguration<UserAdditionalInfo>
    {
        public override void Configure(EntityTypeBuilder<UserAdditionalInfo> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserAdditionalInfo));

            builder.HasIndex(uai => uai.UserId)
                .IsUnique(false);
            builder.Property(uai => uai.UserId)
                .IsRequired();
            builder.Property(uai => uai.IsNotifyEmail)
                .IsRequired();

            builder.HasOne(uai => uai.User)
                .WithOne(u => u.AdditionalInfo)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
