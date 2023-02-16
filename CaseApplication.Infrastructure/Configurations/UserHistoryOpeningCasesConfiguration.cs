﻿using CaseApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseApplication.Infrastructure.Configurations
{
    internal class UserHistoryOpeningCasesConfiguration 
        : BaseEntityConfiguration<UserHistoryOpeningCases>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryOpeningCases> builder)
        {
            base.Configure(builder);

            builder.HasIndex(i => i.UserId)
                .IsUnique(false);
            builder.HasIndex(i => i.GameCaseId)
                .IsUnique(false);
            builder.HasIndex(i => i.GameItemId)
                .IsUnique(false);
        }
    }
}