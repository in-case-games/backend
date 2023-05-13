using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Resources.DAL.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class ReviewImageConfiguration : BaseEntityConfiguration<ReviewImage>
    {
        public override void Configure(EntityTypeBuilder<ReviewImage> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(ReviewImage));

            builder.HasIndex(i => i.ReviewId)
                .IsUnique(false);

            builder.Property(p => p.ImageUri)
                .IsRequired();

            builder.HasOne(o => o.Review)
                .WithMany(m => m.Images)
                .HasForeignKey(fk => fk.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
