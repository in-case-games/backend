using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Support.DAL.Entities;

namespace Support.DAL.Configurations;
internal class BaseEntityConfiguration<TEntity> : 
    IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(k => k.Id);
        builder.HasIndex(i => i.Id).IsUnique();
    }
}