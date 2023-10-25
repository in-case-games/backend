using EmailSender.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EmailSender.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<UserAdditionalInfo> AdditionalInfos => Set<UserAdditionalInfo>();

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
