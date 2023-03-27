using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities;

namespace InCase.Infrastructure.Configurations
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
