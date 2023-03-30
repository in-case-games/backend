using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class PromocodeTypeConfiguration : BaseEntityConfiguration<PromocodeType>
    {
        public override void Configure(EntityTypeBuilder<PromocodeType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(PromocodeType));

            builder.HasIndex(i => i.Name)
                .IsUnique();

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
