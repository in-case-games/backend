using CaseApplication.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class UserHistoryOpeningCasesConfiguration 
        : BaseEntityConfiguration<UserHistoryOpeningCases>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryOpeningCases> builder)
        {
            base.Configure(builder);

            builder.HasIndex(k => k.UserId)
                .IsUnique(false);
            builder.HasIndex(k => k.GameCaseId)
                .IsUnique(false);
            builder.HasIndex(k => k.GameItemId)
                .IsUnique(false);
        }
    }
}
