using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class LootBoxConfiguration : BaseEntityConfiguration<LootBox>
    {
        public override void Configure(EntityTypeBuilder<LootBox> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBox));

            builder.HasIndex(i => i.GameId)
                .IsUnique(false);

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.Cost)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();
            builder.Property(p => p.Balance)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();
            builder.Property(p => p.VirtualBalance)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();
            builder.Property(p => p.ImageUri)
                .IsRequired();
            builder.Property(p => p.IsLocked)
                .IsRequired();

            builder.HasOne(o => o.Game)
                .WithMany(m => m.Boxes)
                .HasForeignKey(fk => fk.GameId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
