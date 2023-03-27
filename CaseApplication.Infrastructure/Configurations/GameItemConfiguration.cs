using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CaseApplication.Domain.Entities.Resources;

namespace CaseApplication.Infrastructure.Configurations
{
    internal class GameItemConfiguration: BaseEntityConfiguration<GameItem>
    {
        public override void Configure(EntityTypeBuilder<GameItem> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.GameItemName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.GameItemType)
                .IsRequired();
            builder.Property(p => p.GameName)
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

            builder.HasMany(m => m.UserInventories)
                .WithOne(o => o.GameItem)
                .HasForeignKey(fk => fk.GameItemId);

            builder.HasMany(m => m.CaseInventories)
                .WithOne(o => o.GameItem)
                .HasForeignKey(fk => fk.GameItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.UserHistoryOpeningCases)
                .WithOne(o => o.GameItem)
                .HasForeignKey(fk => fk.GameItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
