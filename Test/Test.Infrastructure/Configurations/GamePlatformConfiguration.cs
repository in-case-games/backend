using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Domain.Entities;

namespace Test.Infrastructure.Configurations
{
    internal class GamePlatformConfiguration : BaseEntityConfiguration<GamePlatform>
    {
        public override void Configure(EntityTypeBuilder<GamePlatform> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GamePlatform));
        }
    }
}
