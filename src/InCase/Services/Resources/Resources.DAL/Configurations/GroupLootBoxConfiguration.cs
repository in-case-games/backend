using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class GroupLootBoxConfiguration : BaseEntityConfiguration<GroupLootBox>
    {
        public override void Configure(EntityTypeBuilder<GroupLootBox> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GroupLootBox));

            builder.HasIndex(glb => glb.Name)
                .IsUnique();
            builder.Property(glb => glb.Name)
                .IsRequired();
        }
    }
}
