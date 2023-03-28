using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class GamePlatformConfiguration : BaseEntityConfiguration<GamePlatform>
    {
        public override void Configure(EntityTypeBuilder<GamePlatform> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GamePlatform));

            builder.HasIndex(i => i.GameId)
                .IsUnique(false);

            builder.Property(p => p.Uri)
                .IsRequired();
            builder.Property(p => p.DomainUri)
                .IsRequired();
            builder.Property(p => p.Name)
                .IsRequired();

            builder.HasOne(o => o.Game)
                .WithMany(o => o.Platforms)
                .HasForeignKey(o => o.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
