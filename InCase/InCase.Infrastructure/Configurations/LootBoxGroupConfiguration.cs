using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class LootBoxGroupConfiguration : BaseEntityConfiguration<LootBoxGroup>
    {
        public override void Configure(EntityTypeBuilder<LootBoxGroup> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBoxGroup));

            builder.HasIndex(i => i.BoxId)
                .IsUnique(false);
            builder.HasIndex(i => i.GroupId)
                .IsUnique(false);
            builder.HasIndex(i => i.GameId)
                .IsUnique(false);

            builder.HasOne(o => o.Group)
                .WithOne(o => o.Group)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Game)
                .WithMany(m => m.Groups)
                .HasForeignKey(o => o.GameId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Box)
                .WithMany(m => m.Groups)
                .HasForeignKey(o => o.BoxId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
