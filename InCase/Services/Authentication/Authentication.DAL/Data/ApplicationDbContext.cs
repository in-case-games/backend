using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Authentication.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<UserAdditionalInfo> AdditionalInfos => Set<UserAdditionalInfo>();
        public DbSet<UserRestriction> Restrictions => Set<UserRestriction>();
        public DbSet<UserRole> Roles => Set<UserRole>();

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
