using Microsoft.EntityFrameworkCore;
using Support.DAL.Entities;
using System.Reflection;

namespace Support.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AnswerImage> AnswerImages => Set<AnswerImage>();
        public DbSet<SupportTopic> Topics => Set<SupportTopic>();

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
