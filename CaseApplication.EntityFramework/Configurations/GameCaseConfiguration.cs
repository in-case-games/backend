using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class GameCaseConfiguration: BaseEntityConfiguration<GameCase>
    {
        public override void Configure(EntityTypeBuilder<GameCase> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.GameCaseName)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(p => p.GameCaseName)
                .IsUnique();

            builder.HasIndex(i => i.GameCaseName)
                .IsUnique();

            builder.Property(p => p.GameCaseImage)
                .IsRequired();

            builder.Property(p => p.GameCaseCost)
                .HasColumnType("DECIMAL(18, 5)")
                .IsRequired();

            builder.Property(p => p.RevenuePrecentage)
                .HasColumnType("DECIMAL(18, 5)")
                .IsRequired();

            builder.HasMany(p => p.СaseInventories)
                .WithOne(p => p.GameCase)
                .HasForeignKey(p => p.GameCaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.UserHistoryOpeningCases)
                .WithOne(p => p.GameCase)
                .HasForeignKey(p => p.GameCaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
