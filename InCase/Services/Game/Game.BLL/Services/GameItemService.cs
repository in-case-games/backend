using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class GameItemService : IGameItemService
    {
        private readonly ApplicationDbContext _context;

        public GameItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameItemResponse> GetAsync(Guid id)
        {
            GameItem item = await _context.GameItems
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id) ??
                throw new NotFoundException("Предмет не найден");

            return item.ToResponse();
        }

        public async Task<GameItemResponse> CreateAsync(GameItemRequest request, bool isNewGuid = false)
        {
            GameItem item = request.ToEntity(isNewGuid : isNewGuid);

            await _context.GameItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return item.ToResponse();
        }

        public async Task<GameItemResponse> UpdateAsync(GameItemRequest request)
        {
            GameItem item = await _context.GameItems
                .FirstOrDefaultAsync(gi => gi.Id == request.Id) ??
                throw new NotFoundException("Предмет не найден");

            item.Cost = request.Cost;

            await _context.SaveChangesAsync();

            return item.ToResponse();
        }

        public async Task<GameItemResponse> DeleteAsync(Guid id)
        {
            GameItem item = await _context.GameItems
                .AsNoTracking()
                .FirstOrDefaultAsync(gi => gi.Id == id) ??
                throw new NotFoundException("Предмет не найден");

            _context.GameItems.Remove(item);
            await _context.SaveChangesAsync();

            return item.ToResponse();
        }
    }
}
