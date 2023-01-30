using CaseApplication.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseApplication.EntityFramework.Configurations;

internal class PromocodeTypeConfiguration: BaseEntityConfiguration<PromocodeType>
{
    public override void Configure(EntityTypeBuilder<PromocodeType> builder)
    {
        base.Configure(builder);

        builder.HasIndex(i => i.PromocodeTypeName)
            .IsUnique();
    }
}