using Microsoft.EntityFrameworkCore;
using Review.DAL.Entities;
using System.Reflection;

namespace Review.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ReviewImage> Images { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserReview> Reviews { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
