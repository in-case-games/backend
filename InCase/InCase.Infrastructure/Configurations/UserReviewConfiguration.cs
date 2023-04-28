using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InCase.Domain.Entities.Resources;

namespace InCase.Infrastructure.Configurations
{
    internal class UserReviewConfiguration : BaseEntityConfiguration<UserReview>
    {
        public override void Configure(EntityTypeBuilder<UserReview> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserReview));

            builder.HasIndex(i => i.UserId)
                .IsUnique(false);

            builder.Property(p => p.Title)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(p => p.Content)
                .HasMaxLength(120)
                .IsRequired();
            builder.Property(p => p.CreationDate)
                .IsRequired();
            builder.Property(p => p.Score)
                .IsRequired();
            builder.Property(p => p.IsApproved)
                .IsRequired();

            builder.HasOne(o => o.User)
                .WithMany(m => m.Reviews)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
