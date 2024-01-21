using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EmailSender.DAL.Entities;

namespace EmailSender.DAL.Configurations;

internal class BaseEntityConfiguration<TEntity> :
    IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(k => k.Id);
        builder.HasIndex(i => i.Id).IsUnique();
    }
}