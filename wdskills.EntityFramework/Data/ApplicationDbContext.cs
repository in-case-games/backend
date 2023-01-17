using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;

namespace CaseApplication.EntityFramework.Data
{
    public class ApplicationDbContext : DbContext
    {
        internal DbSet<User> User => Set<User>();
        internal DbSet<UserRole> UserRole => Set<UserRole>();
        internal DbSet<UserRestriction> UserRestriction => Set<UserRestriction>();
        internal DbSet<UserAdditionalInfo> UserAdditionalInfo => Set<UserAdditionalInfo>();
        internal DbSet<UserInventory> UserInventory => Set<UserInventory>();
        internal DbSet<GameCase> GameCase => Set<GameCase>();
        internal DbSet<GameItem> GameItem => Set<GameItem>();
        internal DbSet<CaseInventory> CaseInventory => Set<CaseInventory>();

        public ApplicationDbContext(DbContextOptions options) : base(options) {}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-E44T45H\\FERBRAY;Database=CaseApp;Trusted_Connection=True;");
        }
    }
}
