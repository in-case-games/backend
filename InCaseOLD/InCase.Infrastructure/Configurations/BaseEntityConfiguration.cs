﻿using InCase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InCase.Infrastructure.Configurations
{
    internal class BaseEntityConfiguration<TEntity> :
        IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(k => k.Id);
            builder.HasIndex(i => i.Id).IsUnique();
        }
    }
}