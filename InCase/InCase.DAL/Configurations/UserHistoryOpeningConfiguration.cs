using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class UserHistoryOpeningConfiguration : BaseEntityConfiguration<UserHistoryOpening>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryOpening> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserHistoryOpening));

            builder.HasIndex(i => i.UserId)
                .IsUnique(false);
            builder.HasIndex(i => i.BoxId)
                .IsUnique(false);
            builder.HasIndex(i => i.ItemId)
                .IsUnique(false);

            builder.Property(p => p.Date)
                .IsRequired();

            builder.HasOne(o => o.User)
                .WithMany(m => m.HistoryOpenings)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Item)
                .WithMany(m => m.HistoryOpenings)
                .HasForeignKey(o => o.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Box)
                .WithMany(m => m.HistoryOpenings)
                .HasForeignKey(o => o.BoxId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
