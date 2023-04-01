﻿using InCase.Domain.Entities.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InCase.Infrastructure.Configurations
{
    internal class RestrictionTypeConfiguration : BaseEntityConfiguration<RestrictionType>
    {
        private readonly List<RestrictionType> types = new() { 
            new() { Name = "mute" }, new() { Name = "ban" }
        };

        public override void Configure(EntityTypeBuilder<RestrictionType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(RestrictionType));

            builder.HasIndex(i => i.Name)
                .IsUnique();
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(o => o.Restriction)
                .WithOne(m => m.Type)
                .OnDelete(DeleteBehavior.Cascade);

            foreach (var type in types)
                builder.HasData(type);
        }
    }
}
