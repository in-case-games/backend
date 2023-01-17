using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.EntityFramework.Configurations
{
    internal class UserRestrictionConfiguration: BaseEntityConfiguration<UserRestriction>
    {
        public override void Configure(EntityTypeBuilder<UserRestriction> builder)
        {
            base.Configure(builder);

            builder.HasOne(o => o.User)
                .WithMany(m => m.UserRestrictions)
                .HasForeignKey(fk => fk.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.RestrictionName)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(p => p.CreatedDate)
                .HasColumnType("datetime2(7)")
                .HasDefaultValue(DateTime.Now);
        }
    }
}
