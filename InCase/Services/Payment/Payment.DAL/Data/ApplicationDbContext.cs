using Microsoft.EntityFrameworkCore;
using Payment.DAL.Entities;
using System.Reflection;

namespace Payment.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<UserPayments> UserPayments => Set<UserPayments>();
        public DbSet<UserPromocode> UsersPromocodes => Set<UserPromocode>();

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
