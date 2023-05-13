using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources.DAL.Entities;

namespace InCase.Infrastructure.Configurations
{
    internal class ItemWithdrawStatusConfiguration : BaseEntityConfiguration<ItemWithdrawStatus>
    {
        private readonly List<ItemWithdrawStatus> statuses = new() {
            new() { Name = "purchase" }, new() { Name = "waiting" }, 
            new() { Name = "transfer" }, new() { Name = "given" }, 
            new() { Name = "cancel" }
        };

        public override void Configure(EntityTypeBuilder<ItemWithdrawStatus> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(ItemWithdrawStatus));

            builder.HasIndex(i => i.Name)
                .IsUnique();
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            foreach (var status in statuses)
                builder.HasData(status);
        }
    }
}
