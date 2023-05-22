using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class LootBoxGroupConfiguration : BaseEntityConfiguration<LootBoxGroup>
    {
        public override void Configure(EntityTypeBuilder<LootBoxGroup> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBoxGroup));

            builder.HasIndex(lbg => lbg.BoxId)
                .IsUnique(false);
            builder.HasIndex(lbg => lbg.GroupId)
                .IsUnique(false);
            builder.HasIndex(lbg => lbg.GameId)
                .IsUnique(false);

            builder.HasOne(lbg => lbg.Group)
                .WithOne(glb => glb.Group)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(lbg => lbg.Game)
                .WithMany(g => g.Groups)
                .HasForeignKey(lbg => lbg.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(lbg => lbg.Box)
                .WithMany(lb => lb.Groups)
                .HasForeignKey(lbg => lbg.BoxId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
