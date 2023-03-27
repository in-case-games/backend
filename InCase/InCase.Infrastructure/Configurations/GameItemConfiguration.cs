using InCase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InCase.Infrastructure.Configurations
{
    internal class GameItemConfiguration : BaseEntityConfiguration<GameItem>
    {
        public override void Configure(EntityTypeBuilder<GameItem> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItem));
        }
    }
}
