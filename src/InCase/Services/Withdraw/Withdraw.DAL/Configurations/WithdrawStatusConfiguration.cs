using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Withdraw.DAL.Entities;

namespace Withdraw.DAL.Configurations
{
    internal class WithdrawStatusConfiguration : BaseEntityConfiguration<WithdrawStatus>
    {
        private static readonly List<WithdrawStatus> statuses = new() {
            new() { Name = "purchase" }, new() { Name = "transfer" }, 
            new() { Name = "given" }, new() { Name = "cancel" }
        };

        public override void Configure(EntityTypeBuilder<WithdrawStatus> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(WithdrawStatus));

            builder.HasIndex(ws => ws.Name)
                .IsUnique();
            builder.Property(ws => ws.Name)
                .IsRequired();

            foreach(var stat in statuses)
                builder.HasData(stat);
        }
    }
}
