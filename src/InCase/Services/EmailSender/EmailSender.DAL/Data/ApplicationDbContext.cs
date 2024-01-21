using EmailSender.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EmailSender.DAL.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserAdditionalInfo> AdditionalInfos => Set<UserAdditionalInfo>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}