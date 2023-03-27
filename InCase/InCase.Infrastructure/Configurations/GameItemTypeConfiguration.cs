using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class GameItemTypeConfiguration : BaseEntityConfiguration<GameItemType>
    {
        public override void Configure(EntityTypeBuilder<GameItemType> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(GameItemType));
        }
    }
}
