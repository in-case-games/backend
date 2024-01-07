using Game.BLL.Exceptions;
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

        public async Task<GameItem?> GetAsync(Guid id, CancellationToken cancellation = default) => 
            await _context.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(gi => gi.Id == id, cancellation);

        public async Task CreateAsync(GameItemTemplate template, CancellationToken cancellation = default)
        {
            await _context.Items.AddAsync(new GameItem
            {
                Id = template.Id,
                Cost = template.Cost
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(GameItemTemplate template, CancellationToken cancellation = default)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(gi => gi.Id == template.Id, cancellation) ??
                throw new NotFoundException("Предмет не найден");

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
