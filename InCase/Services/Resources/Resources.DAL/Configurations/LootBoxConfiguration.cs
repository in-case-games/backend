using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class LootBoxConfiguration : BaseEntityConfiguration<LootBox>
    {
        public override void Configure(EntityTypeBuilder<LootBox> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBox));

            builder.HasIndex(lb => lb.GameId)
                .IsUnique(false);
            builder.HasIndex(lb => lb.Name)
                .IsUnique();
            builder.HasIndex(lb => lb.HashName)
                .IsUnique();

            builder.Property(lb => lb.Name)
                .IsRequired();
            builder.Property(lb => lb.HashName)
                .IsRequired();
            builder.Property(lb => lb.Cost)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();

            builder.HasOne(lb => lb.Game)
                .WithMany(g => g.Boxes)
                .HasForeignKey(lb => lb.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
