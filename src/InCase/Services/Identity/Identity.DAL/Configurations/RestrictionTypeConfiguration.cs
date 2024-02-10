using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Identity.DAL.Entities;

namespace Identity.DAL.Configurations;
internal class RestrictionTypeConfiguration : BaseEntityConfiguration<RestrictionType>
{
    private readonly List<RestrictionType> _types =
    [
        new RestrictionType { Name = "mute" }, new RestrictionType { Name = "ban" },
        new RestrictionType { Name = "warn" }
    ];

    public override void Configure(EntityTypeBuilder<RestrictionType> builder)
    {
        base.Configure(builder);

        builder.ToTable(nameof(RestrictionType));

        builder.HasIndex(i => i.Name)
            .IsUnique();
        builder.Property(p => p.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(o => o.Restriction)
            .WithOne(m => m.Type)
            .OnDelete(DeleteBehavior.Cascade);

        foreach (var type in _types) builder.HasData(type);
    }
}