using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
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
