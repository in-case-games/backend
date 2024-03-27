using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Authentication.DAL.Data;
public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
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