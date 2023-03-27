using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class GameConfiguration : BaseEntityConfiguration<Game>
    {
        public override void Configure(EntityTypeBuilder<Game> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(Game));
        }
    }
}
