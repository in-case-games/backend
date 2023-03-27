using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities;

namespace InCase.Infrastructure.Configurations
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
