using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wdskills.DomainLayer.Entities;
using wdskills.DomainLayer.Repositories;
using wdskills.EntityFramework.Data;

namespace wdskills.EntityFramework.Repositories
{
    public class GameItemRepository : IGameItemRepository
    {
        private readonly ApplicationDbContextFactory _contextFactory;
        public GameItemRepository(ApplicationDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> CreateItem(GameItem item)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            await _context.GameItem.AddAsync(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteItem(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            GameItem? item = await _context.GameItem.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                _context.GameItem.Remove(item);
                await _context.SaveChangesAsync();
            }
            return (item != null);
        }

        public async Task<GameItem> GetCurrentItem(Guid id)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            return await _context.GameItem.FirstOrDefaultAsync(x => x.Id == id) ?? new();
        }

        public async Task<IEnumerable<GameItem>> GetItems()
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            return await _context.GameItem.ToListAsync();
        }

        public async Task<bool> UpdateItem(GameItem item)
        {
            using ApplicationDbContext _context = _contextFactory.CreateDbContext();
            _context.Set<GameItem>().Update(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
