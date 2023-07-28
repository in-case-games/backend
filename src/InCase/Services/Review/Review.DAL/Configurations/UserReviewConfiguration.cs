using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Review.DAL.Entities;

namespace Review.DAL.Configurations
{
    internal class UserReviewConfiguration : BaseEntityConfiguration<UserReview>
    {
        public override void Configure(EntityTypeBuilder<UserReview> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(UserReview));

            builder.HasIndex(ur => ur.UserId)
                .IsUnique(false);

            builder.Property(ur => ur.Title)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(ur => ur.Content)
                .HasMaxLength(120)
                .IsRequired();
            builder.Property(ur => ur.CreationDate)
                .IsRequired();
            builder.Property(ur => ur.Score)
                .IsRequired();
            builder.Property(ur => ur.IsApproved)
                .IsRequired();
            builder.Property(ur => ur.UserId)
                .IsRequired();

            builder.HasOne(ur => ur.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
