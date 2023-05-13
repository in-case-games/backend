using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class PromocodeConfiguration : BaseEntityConfiguration<Promocode>
    {
        public override void Configure(EntityTypeBuilder<Promocode> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(Promocode));

            builder.HasIndex(i => i.Name)
                .IsUnique();

            builder.HasIndex(i => i.TypeId)
                .IsUnique(false);

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.Discount)
                .HasColumnType("DECIMAL(5,5)")
                .IsRequired();
            builder.Property(p => p.NumberActivations)
                .IsRequired();
            builder.Property(p => p.ExpirationDate)
                .IsRequired();

            builder.HasOne(o => o.Type)
                .WithOne(o => o.Promocode)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
