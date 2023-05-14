using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
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
