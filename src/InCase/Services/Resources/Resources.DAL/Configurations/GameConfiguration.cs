using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class GameConfiguration : BaseEntityConfiguration<Game>
    {
        private readonly List<Game> _games = new() {
            new Game { Name = "csgo" }, new Game { Name = "dota2" }
        };

        public override void Configure(EntityTypeBuilder<Game> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(Game));

            builder.HasIndex(g => g.Name)
                .IsUnique();
            builder.Property(g => g.Name)
                .IsRequired();

            foreach (var game in _games) builder.HasData(game);
        }
    }
}
