using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class SupportTopicAnswerConfiguration : BaseEntityConfiguration<SupportTopicAnswer>
    {
        public override void Configure(EntityTypeBuilder<SupportTopicAnswer> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(SupportTopicAnswer));

            builder.Property(p => p.Content)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(p => p.Date)
                .IsRequired();

            builder.HasIndex(i => i.PlaintiffId)
                .IsUnique(false);
            builder.HasIndex(i => i.TopicId)
                .IsUnique(false);

            builder.HasOne(o => o.Topic)
                .WithMany(m => m.Answers)
                .HasForeignKey(fk => fk.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Plaintiff)
                .WithMany(m => m.Answers)
                .HasForeignKey(fk => fk.PlaintiffId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
