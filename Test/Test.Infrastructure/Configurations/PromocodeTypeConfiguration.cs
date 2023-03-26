using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class PromocodeTypeConfiguration : BaseEntityConfiguration<PromocodeType>
    {
        public override void Configure(EntityTypeBuilder<PromocodeType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(PromocodeType));
        }
    }
}
