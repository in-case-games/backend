using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.DAL.Configurations;
internal class LootBoxConfiguration : BaseEntityConfiguration<LootBox>
{
    public override void Configure(EntityTypeBuilder<LootBox> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(LootBox));

        builder.Property(lb => lb.IsLocked)
            .IsRequired();
        builder.Property(lb => lb.ExpirationBannerDate)
            .IsRequired(false);

        builder.Property(lb => lb.Cost)
            .HasColumnType("DECIMAL(18,5)")
            .IsRequired();
        builder.Property(lb => lb.Balance)
            .HasColumnType("DECIMAL(18,5)")
            .IsRequired();
        builder.Property(lb => lb.VirtualBalance)
            .HasColumnType("DECIMAL(18,5)")
            .IsRequired();
    }
}