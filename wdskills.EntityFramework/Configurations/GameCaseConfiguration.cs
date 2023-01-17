using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.DomainLayer.Entities;

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

            builder.Property(p => p.GameCaseImage)
                .IsRequired();

            builder.Property(p => p.GameCaseCost)
                .HasPrecision(18, 5)
                .IsRequired();

            builder.Property(p => p.RevenuePrecentage)
                .HasPrecision(18, 5)
                .IsRequired();
        }
    }
}
