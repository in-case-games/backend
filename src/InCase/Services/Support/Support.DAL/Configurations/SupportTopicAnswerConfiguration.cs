using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Support.DAL.Entities;

namespace Support.DAL.Configurations
{
    internal class SupportTopicAnswerConfiguration : BaseEntityConfiguration<SupportTopicAnswer>
    {
        public override void Configure(EntityTypeBuilder<SupportTopicAnswer> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(SupportTopicAnswer));

            builder.Property(sta => sta.Content)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(sta => sta.Date)
                .IsRequired();

            builder.HasIndex(sta => sta.PlaintiffId)
                .IsUnique(false);
            builder.HasIndex(sta => sta.TopicId)
                .IsUnique(false);

            builder.HasOne(sta => sta.Topic)
                .WithMany(st => st.Answers)
                .HasForeignKey(sta => sta.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sta => sta.Plaintiff)
                .WithMany(u => u.Answers)
                .HasForeignKey(sta => sta.PlaintiffId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
