using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class GameConfiguration : BaseEntityConfiguration<Game>
    {
        private readonly List<Game> games = new() {
            new() { Name = "csgo" }, new() { Name = "dota2" }
        };

        public override void Configure(EntityTypeBuilder<Game> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(Game));

            builder.HasIndex(i => i.Name)
                .IsUnique();
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach(var game in games)
                builder.HasData(game);
        }
    }
}
