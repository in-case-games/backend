using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class SupportTopicConfiguration : BaseEntityConfiguration<SupportTopic>
    {
        public override void Configure(EntityTypeBuilder<SupportTopic> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(SupportTopic));

            builder.Property(p => p.Title)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.Content)
                .HasMaxLength(120)
                .IsRequired();
            builder.Property(p => p.Date)
                .IsRequired();
            builder.Property(p => p.IsClosed)
                .IsRequired();

            builder.HasIndex(i => i.UserId)
                .IsUnique(false);
            builder.HasIndex(i => i.SupportId)
                .IsUnique(false);

            builder.HasOne(o => o.User)
                .WithMany(m => m.UserTopics)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(o => o.Support)
                .WithMany(m => m.SupportTopics)
                .HasForeignKey(fk => fk.SupportId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
