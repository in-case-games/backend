using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class GameItemConfiguration: BaseEntityConfiguration<GameItem>
    {
        public override void Configure(EntityTypeBuilder<GameItem> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.GameItemName)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(i => i.GameItemName)
                .IsUnique();

            builder.Property(p => p.GameItemCost)
                .HasColumnType("DECIMAL(18, 5)")
                .IsRequired();

            builder.Property(p => p.GameItemImage)
                .IsRequired();

            builder.Property(p => p.GameItemRarity)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasMany(p => p.UserInventories)
                .WithOne(p => p.GameItem)
                .HasForeignKey(p => p.GameItemId);

            builder.HasMany(p => p.CaseInventories)
                .WithOne(P => P.GameItem)
                .HasForeignKey(p => p.GameItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
