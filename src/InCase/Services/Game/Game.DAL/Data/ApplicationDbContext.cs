﻿using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Game.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<GameItem> Items => Set<GameItem>();
        public DbSet<LootBox> Boxes => Set<LootBox>();
        public DbSet<LootBoxInventory> BoxInventories => Set<LootBoxInventory>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserAdditionalInfo> AdditionalInfos => Set<UserAdditionalInfo>();
        public DbSet<UserOpening> Openings => Set<UserOpening>();
        public DbSet<UserPromocode> UserPromocodes => Set<UserPromocode>();
        public DbSet<UserPathBanner> PathBanners => Set<UserPathBanner>();

        public ApplicationDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}