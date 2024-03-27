using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Support.DAL.Entities;

namespace Support.DAL.Configurations;
internal class SupportTopicConfiguration : BaseEntityConfiguration<SupportTopic>
{
    public override void Configure(EntityTypeBuilder<SupportTopic> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(SupportTopic));

        builder.Property(st => st.Title)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(st => st.Content)
            .HasMaxLength(120)
            .IsRequired();
        builder.Property(st => st.Date)
            .IsRequired();
        builder.Property(st => st.IsClosed)
            .IsRequired();

        builder.HasIndex(st => st.UserId)
            .IsUnique(false);

        builder.HasOne(st => st.User)
            .WithMany(u => u.Topics)
            .HasForeignKey(st => st.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}