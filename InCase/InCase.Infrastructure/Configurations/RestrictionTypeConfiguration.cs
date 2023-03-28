using InCase.Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InCase.Infrastructure.Configurations
{
    internal class RestrictionTypeConfiguration : BaseEntityConfiguration<RestrictionType>
    {
        public override void Configure(EntityTypeBuilder<RestrictionType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(RestrictionType));

            builder.HasIndex(i => i.Name)
                .IsUnique();

            builder.HasOne(o => o.Restriction)
                .WithOne(m => m.Type)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
