using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;
using Withdraw.BLL.Exceptions;
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

        public async Task<GameItem?> GetAsync(Guid id, CancellationToken cancellation = default) => 
            await _context.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(gi => gi.Id == id, cancellation);

        public async Task CreateAsync(GameItemTemplate template, CancellationToken cancellation = default)
        {
            var item = new GameItem
            {
                Id = template.Id,
                Cost = template.Cost,
                IdForMarket = template.IdForMarket
            };

            var game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Name == template.GameName, cancellation) ?? 
                throw new NotFoundException("Игра не найдена");

            item.GameId = game.Id;

            await _context.Items.AddAsync(item, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(GameItemTemplate template, CancellationToken cancellation = default)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(gi => gi.Id == template.Id, cancellation) ??
                throw new NotFoundException("Предмет не найден");

            var game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Name == template.GameName, cancellation) ??
                throw new NotFoundException("Игра не найдена");

            if (template.IdForMarket is not null) item.IdForMarket = template.IdForMarket;

            item.GameId = game.Id;
            item.Cost = template.Cost;

            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var item = await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id, cancellation) ??
                throw new NotFoundException("Предмет не найден");

            _context.Items.Remove(item);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
