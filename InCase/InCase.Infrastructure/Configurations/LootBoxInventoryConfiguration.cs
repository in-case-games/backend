﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class LootBoxInventoryConfiguration : BaseEntityConfiguration<LootBoxInventory>
    {
        public override void Configure(EntityTypeBuilder<LootBoxInventory> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBoxInventory));
        }
    }
}