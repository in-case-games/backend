using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace Resources.DAL.Configurations
{
    internal class UserHistoryPromocodeConfiguration : BaseEntityConfiguration<UserHistoryPromocode>
    {
        public override void Configure(EntityTypeBuilder<UserHistoryPromocode> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserHistoryPromocode));

            builder.HasIndex(i => i.UserId)
                .IsUnique(false);
            builder.HasIndex(i => i.PromocodeId)
                .IsUnique(false);

            builder.Property(p => p.Date)
                .IsRequired(false);
            builder.Property(p => p.IsActivated)
                .IsRequired();

            builder.HasOne(o => o.Promocode)
                .WithMany(m => m.History)
                .HasForeignKey(fk => fk.PromocodeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.User)
                .WithMany(m => m.HistoryPromocodes)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
