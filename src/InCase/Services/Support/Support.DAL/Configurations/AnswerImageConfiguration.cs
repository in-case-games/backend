using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Support.DAL.Entities;

namespace Support.DAL.Configurations;

internal class AnswerImageConfiguration : BaseEntityConfiguration<AnswerImage>
{
    public override void Configure(EntityTypeBuilder<AnswerImage> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(AnswerImage));

        builder.HasIndex(ai => ai.AnswerId)
            .IsUnique(false);

        builder.HasOne(ai => ai.Answer)
            .WithMany(sta => sta.Images)
            .HasForeignKey(ai => ai.AnswerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}