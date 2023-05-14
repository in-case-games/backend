using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class LootBoxBannerConfiguration : BaseEntityConfiguration<LootBoxBanner>
    {
        public override void Configure(EntityTypeBuilder<LootBoxBanner> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBoxBanner));

            builder.HasIndex(i => i.BoxId)
                .IsUnique();

            builder.Property(p => p.IsActive)
                .IsRequired();
            builder.Property(p => p.CreationDate)
                .IsRequired();
            builder.Property(p => p.ImageUri)
                .IsRequired();
            builder.Property(p => p.ExpirationDate)
                .IsRequired(false);

            builder.HasOne(o => o.Box)
                .WithOne(o => o.Banner)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
