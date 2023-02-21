﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.Domain.Entities.Resources;

namespace CaseApplication.Infrastructure.Configurations
{
    internal class UserRestrictionConfiguration: BaseEntityConfiguration<UserRestriction>
    {
        public override void Configure(EntityTypeBuilder<UserRestriction> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.RestrictionName)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
