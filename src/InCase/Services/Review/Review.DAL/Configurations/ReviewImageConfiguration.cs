using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Review.DAL.Entities;

namespace Review.DAL.Configurations
{
    internal class ReviewImageConfiguration : BaseEntityConfiguration<ReviewImage>
    {
        public override void Configure(EntityTypeBuilder<ReviewImage> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(ReviewImage));

            builder.Property(ri => ri.ReviewId)
                .IsRequired();
            builder.HasIndex(ri => ri.ReviewId)
                .IsUnique(false);

            builder.HasOne(ri => ri.Review)
                .WithMany(ur => ur.Images)
                .HasForeignKey(ri => ri.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
