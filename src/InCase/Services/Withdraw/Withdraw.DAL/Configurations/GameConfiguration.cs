using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Withdraw.DAL.Entities;

namespace Withdraw.DAL.Configurations
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

            builder.HasIndex(g => g.Name)
                .IsUnique();

            builder.Property(g => g.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach(var game in games)
                builder.HasData(game);
        }
    }
}
