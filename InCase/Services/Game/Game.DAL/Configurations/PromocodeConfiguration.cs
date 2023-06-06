using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.DAL.Configurations
{
    internal class PromocodeConfiguration : BaseEntityConfiguration<Promocode>
    {
        public override void Configure(EntityTypeBuilder<Promocode> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(Promocode));

            builder.Property(gi => gi.Discount)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();
        }
    }
}
