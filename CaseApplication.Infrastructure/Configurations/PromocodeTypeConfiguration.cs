using CaseApplication.Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseApplication.Infrastructure.Configurations
{
    internal class PromocodeTypeConfiguration: BaseEntityConfiguration<PromocodeType>
    {
        public override void Configure(EntityTypeBuilder<PromocodeType> builder)
        {
            base.Configure(builder);

            builder.HasIndex(i => i.PromocodeTypeName)
                .IsUnique();
        }
    }
}