using Microsoft.EntityFrameworkCore;
using Identity.DAL.Entities;

namespace Identity.DAL.Data
{
    public class IdentityDbContext: DbContext
    {
        public DbSet<RestrictionType> RestrictionTypes => Set<RestrictionType>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserAdditionalInfo> UserAdditionalInfos => Set<UserAdditionalInfo>();
        public DbSet<UserRestriction> UserRestrictions => Set<UserRestriction>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public IdentityDbContext(DbContextOptions options): base(options) {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}