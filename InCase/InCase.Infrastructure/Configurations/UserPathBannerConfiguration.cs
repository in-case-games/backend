using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserPathBannerConfiguration : BaseEntityConfiguration<UserPathBanner>
    {
        public override void Configure(EntityTypeBuilder<UserPathBanner> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserPathBanner));

            builder.Property(p => p.Date)
                .IsRequired();
            builder.Property(p => p.NumberSteps)
                .IsRequired();

            builder.HasOne(o => o.User)
                .WithMany(m => m.Paths)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Item)
                .WithMany(m => m.PathBanners)
                .HasForeignKey(o => o.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Banner)
                .WithMany(m => m.Paths)
                .HasForeignKey(o => o.BannerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
