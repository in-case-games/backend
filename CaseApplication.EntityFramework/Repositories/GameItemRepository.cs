﻿using Microsoft.EntityFrameworkCore;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using CaseApplication.EntityFramework.Data;

namespace CaseApplication.EntityFramework.Repositories
{
    public class GameItemRepository : IGameItemRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public GameItemRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GameItem> Get(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            GameItem? searchGameItem = await context.GameItem.FirstOrDefaultAsync(x => x.Id == id);

            return searchGameItem ?? throw new Exception("There is no such item, " +
                "review what data comes from the api");
        }

        public async Task<IEnumerable<GameItem>> GetAll()
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            return await context.GameItem.ToListAsync();
        }

        public async Task<bool> Create(GameItem item)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            item.Id = Guid.NewGuid();

            await context.GameItem.AddAsync(item);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(GameItem item)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            GameItem? searchItem = await context.GameItem.FirstOrDefaultAsync(x => x.Id == item.Id);

            if (searchItem is null) throw new Exception("There is no such item in the database, " +
                "review what data comes from the api");

            context.Entry(searchItem).CurrentValues.SetValues(item);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();

            GameItem? searchItem = await context.GameItem.FirstOrDefaultAsync(x => x.Id == id);

            if (searchItem is null) throw new Exception("There is no such item in the database, " +
                "review what data comes from the api");

            context.GameItem.Remove(searchItem);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
