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

            builder.Property(lbb => lbb.CreationDate)
                .IsRequired();
            builder.Property(lbb => lbb.ExpirationDate)
                .IsRequired(false);

            builder.HasOne(lbb => lbb.Box)
                .WithOne(lb => lb.Banner)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
