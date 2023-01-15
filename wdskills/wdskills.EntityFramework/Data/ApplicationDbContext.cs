using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wdskills.DomainLayer.Domain;

namespace wdskills.EntityFramework.Data
{
    public class ApplicationDbContext : DbContext
    {
        internal DbSet<User> User => Set<User>();
        internal DbSet<UserRole> UserRole => Set<UserRole>();
        internal DbSet<UserEffect> UserEffect => Set<UserEffect>();
        internal DbSet<UserAdditionalInfo> UserAdditionalInfo => Set<UserAdditionalInfo>();
        internal DbSet<UserInventory> UserInventory => Set<UserInventory>();
        internal DbSet<GameCase> GameCase => Set<GameCase>();
        internal DbSet<GameItem> GameItem => Set<GameItem>();

        public ApplicationDbContext(DbContextOptions options) : base(options) {}
    }
}
