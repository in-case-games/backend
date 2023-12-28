using Game.BLL.Exceptions;
using Game.BLL.Interfaces;
using Game.BLL.MassTransit;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class LootBoxService : ILootBoxService
    {
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public LootBoxService(ApplicationDbContext context, BasePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<LootBox?> GetAsync(Guid id, CancellationToken cancellation = default) => 
            await _context.Boxes
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == id, cancellation);

        public async Task CreateAsync(LootBoxTemplate template, CancellationToken cancellation = default)
        {
            await _context.Boxes.AddAsync(new LootBox
            {
                Id = template.Id,
                Cost = template.Cost,
                IsLocked = template.IsLocked,
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(LootBoxTemplate template, CancellationToken cancellation = default)
        {
            var old = await _context.Boxes
                .FirstOrDefaultAsync(lb => lb.Id == template.Id, cancellation) ??
                throw new NotFoundException("Кейс не найден");

            _context.Entry(old).CurrentValues.SetValues(new LootBox
            {
                Id = template.Id,
                Cost = template.Cost,
                IsLocked = template.IsLocked,
                ExpirationBannerDate = old.ExpirationBannerDate,
                Balance = old.Balance,
                VirtualBalance = old.VirtualBalance
            });
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateExpirationBannerAsync(LootBoxBannerTemplate template, CancellationToken cancellation = default)
        {
            var box = await _context.Boxes
                .FirstOrDefaultAsync(lb => lb.Id == template.BoxId, cancellation) ??
                throw new NotFoundException("Кейс не найден");

            box.ExpirationBannerDate = template.ExpirationDate;

            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var box = await _context.Boxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id, cancellation) ??
                throw new NotFoundException("Кейс не найден");

            _context.Boxes.Remove(box);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
