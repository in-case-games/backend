using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class SupportTopicAnswerConfiguration : BaseEntityConfiguration<SupportTopicAnswer>
    {
        public override void Configure(EntityTypeBuilder<SupportTopicAnswer> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(SupportTopicAnswer));
        }
    }
}
