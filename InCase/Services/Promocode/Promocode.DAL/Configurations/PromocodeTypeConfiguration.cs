using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promocode.DAL.Entities;

namespace Promocode.DAL.Configurations
{
    internal class PromocodeTypeConfiguration : BaseEntityConfiguration<PromocodeType>
    {
        public override void Configure(EntityTypeBuilder<PromocodeType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(PromocodeType));

            builder.Property(pt => pt.Name)
                .IsRequired();
            builder.HasIndex(pt => pt.Name)
                .IsUnique();
        }
    }
}
