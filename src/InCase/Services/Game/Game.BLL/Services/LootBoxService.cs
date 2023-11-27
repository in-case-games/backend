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
    public class LootBoxService : ILootBoxService
    {
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public LootBoxService(ApplicationDbContext context, BasePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<LootBox?> GetAsync(Guid id) => await _context.Boxes
            .AsNoTracking()
            .FirstOrDefaultAsync(lb => lb.Id == id);

        public async Task CreateAsync(LootBoxTemplate template)
        {
            LootBox box = template.ToEntity();

            await _context.Boxes.AddAsync(box);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LootBoxTemplate template)
        {
            LootBox boxOld = await _context.Boxes
                .FirstOrDefaultAsync(lb => lb.Id == template.Id) ??
                throw new NotFoundException("Кейс не найден");

            LootBox boxNew = template.ToEntity();
            boxNew.ExpirationBannerDate = boxOld.ExpirationBannerDate;;

            _context.Entry(boxOld).CurrentValues.SetValues(boxNew);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateExpirationBannerAsync(LootBoxBannerTemplate template)
        {
            LootBox box = await _context.Boxes
                .FirstOrDefaultAsync(lb => lb.Id == template.BoxId) ??
                throw new NotFoundException("Кейс не найден");

            box.ExpirationBannerDate = template.ExpirationDate;
            _context.Entry(box).Property(p => p.ExpirationBannerDate).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            LootBox box = await _context.Boxes
                .AsNoTracking()
                .FirstOrDefaultAsync(lb => lb.Id == id) ??
                throw new NotFoundException("Кейс не найден");

            _context.Boxes.Remove(box);
            await _context.SaveChangesAsync();
        }
    }
}
