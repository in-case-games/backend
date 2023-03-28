using InCase.Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InCase.Infrastructure.Configurations
{
    internal class NewsImageConfiguration : BaseEntityConfiguration<NewsImage>
    {
        public override void Configure(EntityTypeBuilder<NewsImage> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(NewsImage));

            builder.HasIndex(i => i.NewsId)
                .IsUnique(false);

            builder.Property(p => p.Uri)
                .IsRequired();

            builder.HasOne(o => o.News)
                .WithMany(m => m.Images)
                .HasForeignKey(fk => fk.NewsId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
