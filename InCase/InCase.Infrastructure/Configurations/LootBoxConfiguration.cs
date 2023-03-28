using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class LootBoxConfiguration : BaseEntityConfiguration<LootBox>
    {
        public override void Configure(EntityTypeBuilder<LootBox> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(LootBox));

            builder.HasIndex(i => i.GameId)
                .IsUnique(false);

            builder.Property(p => p.Name)
                .IsRequired();
            builder.Property(p => p.Cost)
                .IsRequired();
            builder.Property(p => p.Balance)
                .IsRequired();
            builder.Property(p => p.VirtualBalance)
                .IsRequired();
            builder.Property(p => p.Uri)
                .IsRequired();
            builder.Property(p => p.IsLocked)
                .IsRequired();

            builder.HasOne(o => o.Game)
                .WithMany(m => m.Boxes)
                .HasForeignKey(fk => fk.GameId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
