using CaseApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseApplication.Infrastructure.Configurations
{
    internal class NewsConfiguration: BaseEntityConfiguration<News>
    {
        public override void Configure(EntityTypeBuilder<News> builder)
        {
            base.Configure(builder);

        }
    }
}
