using Microsoft.EntityFrameworkCore;
using Payment.DAL.Entities;
using System.Reflection;

namespace Payment.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<UserPayment> Payments => Set<UserPayment>();
        public DbSet<UserPromocode> UserPromocodes => Set<UserPromocode>();

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
