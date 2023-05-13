﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class GameItemConfiguration : BaseEntityConfiguration<GameItem>
    {
        public override void Configure(EntityTypeBuilder<GameItem> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItem));

            builder.HasIndex(i => i.GameId)
                .IsUnique(false);
            builder.HasIndex(i => i.TypeId)
                .IsUnique(false);
            builder.HasIndex(i => i.RarityId)
                .IsUnique(false);
            builder.HasIndex(i => i.QualityId)
                .IsUnique(false);

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.HashName)
                .IsRequired();
            builder.Property(p => p.Cost)
                .HasColumnType("DECIMAL(18,5)")
                .IsRequired();
            builder.Property(p => p.ImageUri)
                .IsRequired();
            builder.Property(p => p.IdForMarket)
                .IsRequired(false);

            builder.HasOne(o => o.Game)
                .WithMany(m => m.Items)
                .HasForeignKey(fk => fk.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Rarity)
                .WithOne(o => o.Item)
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(o => o.Quality)
                .WithOne(o => o.Item)
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(o => o.Type)
                .WithOne(o => o.Item)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
