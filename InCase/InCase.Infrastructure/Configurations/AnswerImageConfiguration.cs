using InCase.Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InCase.Infrastructure.Configurations
{
    internal class AnswerImageConfiguration : BaseEntityConfiguration<AnswerImage>
    {
        public override void Configure(EntityTypeBuilder<AnswerImage> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(AnswerImage));

            builder.HasIndex(i => i.AnswerId)
                .IsUnique(false);

            builder.Property(p => p.Uri)
                .IsRequired();

            builder.HasOne(o => o.Answer)
                .WithMany(m => m.Images)
                .HasForeignKey(fk => fk.AnswerId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
