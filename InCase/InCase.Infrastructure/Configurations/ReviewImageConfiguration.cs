using InCase.Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

            builder.Property(p => p.Uri)
                .IsRequired();

            builder.HasOne(o => o.Review)
                .WithMany(m => m.Images)
                .HasForeignKey(fk => fk.ReviewId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
