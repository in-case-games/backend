using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class PromocodeTypeConfiguration : BaseEntityConfiguration<PromocodeType>
    {
        private readonly List<PromocodeType> types = new() {
            new() { Name = "balance" }, new() { Name = "case" }
        };

        public override void Configure(EntityTypeBuilder<PromocodeType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(PromocodeType));

            builder.HasIndex(i => i.Name)
                .IsUnique();

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach (var type in types)
                builder.HasData(type);
        }
    }
}
