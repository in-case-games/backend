using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class PromocodeConfiguration : BaseEntityConfiguration<Promocode>
    {
        public override void Configure(EntityTypeBuilder<Promocode> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(Promocode));
        }
    }
}
