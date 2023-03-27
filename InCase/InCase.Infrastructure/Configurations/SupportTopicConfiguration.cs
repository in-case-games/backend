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

            builder.HasOne(x => x.User)
                .WithMany(t => t.UserTopics)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Support)
                .WithMany(t => t.SupportTopics)
                .HasForeignKey(m => m.SupportId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable(nameof(SupportTopic));
        }
    }
}
