using CaseApplication.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseApplication.EntityFramework.Configurations;

internal class PromocodesUsedByUserConfiguration: BaseEntityConfiguration<PromocodesUsedByUser>
{
    public override void Configure(EntityTypeBuilder<PromocodesUsedByUser> builder)
    {
        base.Configure(builder);
        
        builder.HasIndex(i => i.UserId)
            .IsUnique(false);
        builder.HasIndex(i => i.PromocodeId)
            .IsUnique(false);
    }
}