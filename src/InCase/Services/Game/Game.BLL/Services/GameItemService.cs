using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.MassTransit;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class GameItemService : IGameItemService
    {
        private readonly ApplicationDbContext _context;

        public GameItemService(ApplicationDbContext context, BasePublisher publisher)
        {
            _context = context;
        }

        public async Task<GameItem?> GetAsync(Guid id) => await _context.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(gi => gi.Id == id);

        public async Task<GameItem> CreateAsync(GameItemTemplate template)
        {
            GameItem item = template.ToEntity();

            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task UpdateAsync(GameItemTemplate template)
        {
            GameItem item = await _context.Items
                .FirstOrDefaultAsync(gi => gi.Id == template.Id) ??
                throw new NotFoundException("Предмет не найден");

            item.Cost = template.Cost;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            GameItem item = await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id) ??
                throw new NotFoundException("Предмет не найден");

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
