using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class NewsConfiguration : BaseEntityConfiguration<News>
    {
        public override void Configure(EntityTypeBuilder<News> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(News));
        }
    }
}
