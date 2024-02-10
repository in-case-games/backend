using Microsoft.EntityFrameworkCore;
using Identity.DAL.Entities;
using System.Reflection;

namespace Identity.DAL.Data;
public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<RestrictionType> RestrictionTypes => Set<RestrictionType>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserAdditionalInfo> UserAdditionalInfos => Set<UserAdditionalInfo>();
    public DbSet<UserRestriction> UserRestrictions => Set<UserRestriction>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}