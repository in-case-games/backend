using CaseApplication.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class NewsConfiguration: BaseEntityConfiguration<News>
    {
        public override void Configure(EntityTypeBuilder<News> builder)
        {
            base.Configure(builder);

        }
    }
}
