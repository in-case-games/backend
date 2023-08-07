using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Exceptions;
using Withdraw.BLL.Helpers;
using Withdraw.BLL.Interfaces;
using Withdraw.DAL.Data;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Services
{
    public class GameItemService : IGameItemService
    {
        private readonly ApplicationDbContext _context;

        public GameItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameItem?> GetAsync(Guid id) => await _context.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(gi => gi.Id == id);

        public async Task CreateAsync(GameItemTemplate template)
        {
            GameItem item = template.ToEntity();

            Game game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Name == template.GameName) ?? 
                throw new NotFoundException("Игра не найдена");

            item.GameId = game.Id;

            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GameItemTemplate template)
        {
            GameItem item = await _context.Items
                .FirstOrDefaultAsync(gi => gi.Id == template.Id) ??
                throw new NotFoundException("Предмет не найден");

            Game game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Name == template.GameName) ??
                throw new NotFoundException("Игра не найдена");

            item.GameId = game.Id;
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
