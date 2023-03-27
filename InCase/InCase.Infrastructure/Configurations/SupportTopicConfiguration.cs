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
        }
    }
}
