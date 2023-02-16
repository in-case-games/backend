using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseApplication.Infrastructure.Configurations
{
    internal class GameCaseConfiguration: BaseEntityConfiguration<GameCase>
    {
        public override void Configure(EntityTypeBuilder<GameCase> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.GameCaseName)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(p => p.GroupCasesName)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(i => i.GameCaseName)
                .IsUnique();

            builder.Property(p => p.GameCaseImage)
                .IsRequired();

            builder.Property(p => p.GameCaseCost)
                .HasColumnType("DECIMAL(18, 5)")
                .IsRequired();

            builder.Property(p => p.GameCaseBalance)
                .HasColumnType("DECIMAL(18, 5)")
                .IsRequired();

            builder.Property(p => p.RevenuePrecentage)
                .HasColumnType("DECIMAL(18, 5)")
                .IsRequired();

            builder.HasMany(m => m.СaseInventories)
                .WithOne(o => o.GameCase)
                .HasForeignKey(fk => fk.GameCaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.UserHistoryOpeningCases)
                .WithOne(o => o.GameCase)
                .HasForeignKey(fk => fk.GameCaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
