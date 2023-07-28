using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Promocode.DAL.Entities;

namespace Promocode.DAL.Configurations
{
    internal class PromocodeTypeConfiguration : BaseEntityConfiguration<PromocodeType>
    {
        private readonly List<PromocodeType> types = new() {
            new() { Name = "balance" }, new() { Name = "box" }
        };

        public override void Configure(EntityTypeBuilder<PromocodeType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(PromocodeType));

            builder.Property(pt => pt.Name)
                .IsRequired();
            builder.HasIndex(pt => pt.Name)
                .IsUnique();

            foreach(var type in types)
                builder.HasData(type);
        }
    }
}
