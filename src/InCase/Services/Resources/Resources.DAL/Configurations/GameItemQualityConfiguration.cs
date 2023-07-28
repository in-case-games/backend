﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class GameItemQualityConfiguration : BaseEntityConfiguration<GameItemQuality>
    {
        private readonly List<GameItemQuality> qualities = new() {
            new() { Name = "none" }, new() { Name = "battle scarred" },
            new() { Name = "well worn" }, new() { Name = "field tested" },
            new() { Name = "minimal wear" }, new() { Name = "factory new" },
        };

        public override void Configure(EntityTypeBuilder<GameItemQuality> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemQuality));

            builder.HasIndex(giq => giq.Name)
                .IsUnique();

            builder.Property(giq => giq.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach (var quality in qualities)
                builder.HasData(quality);
        }
    }
}