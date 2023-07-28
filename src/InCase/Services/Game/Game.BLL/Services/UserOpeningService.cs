using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.DAL.Data;
using Game.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class UserOpeningService : IUserOpeningService
    {
        private readonly ApplicationDbContext _context;

        public UserOpeningService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserOpeningResponse> GetAsync(Guid id)
        {
            UserOpening opening = await _context.Openings
                .AsNoTracking()
                .FirstOrDefaultAsync(uo => uo.Id == id) ?? 
                throw new NotFoundException("История открытия не найдена");

            return opening.ToResponse();
        }

        public async Task<List<UserOpeningResponse>> GetAsync(int count)
        {
            List<UserOpening> openings = await _context.Openings
                .AsNoTracking()
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync();

            return openings.ToResponse();
        }

        public async Task<List<UserOpeningResponse>> GetAsync(Guid userId, int count)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            List<UserOpening> openings = await _context.Openings
                .AsNoTracking()
                .Where(uo => uo.UserId == userId)
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync();

            return openings.ToResponse();
        }

        public async Task<List<UserOpeningResponse>> GetByBoxIdAsync(Guid userId, Guid boxId, int count)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");
            if (!await _context.Boxes.AnyAsync(lb => lb.Id == boxId))
                throw new NotFoundException("Кейс не найден");

            List<UserOpening> openings = await _context.Openings
                .AsNoTracking()
                .Where(uo => uo.UserId == userId && uo.BoxId == boxId)
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync();

            return openings.ToResponse();
        }

        public async Task<List<UserOpeningResponse>> GetByBoxIdAsync(Guid boxId, int count)
        {
            if (!await _context.Boxes.AnyAsync(lb => lb.Id == boxId))
                throw new NotFoundException("Кейс не найден");

            List<UserOpening> openings = await _context.Openings
                .AsNoTracking()
                .Where(uo => uo.BoxId == boxId)
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync();

            return openings.ToResponse();
        }

        public async Task<List<UserOpeningResponse>> GetByItemIdAsync(Guid userId, Guid itemId, int count)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");
            if (!await _context.Items.AnyAsync(gi => gi.Id == itemId))
                throw new NotFoundException("Предмет не найден");

            List<UserOpening> openings = await _context.Openings
                .AsNoTracking()
                .Where(uo => uo.UserId == userId && uo.ItemId == itemId)
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync();

            return openings.ToResponse();
        }

        public async Task<List<UserOpeningResponse>> GetByItemIdAsync(Guid itemId, int count)
        {
            if (!await _context.Items.AnyAsync(gi => gi.Id == itemId))
                throw new NotFoundException("Предмет не найден");

            List<UserOpening> openings = await _context.Openings
                .AsNoTracking()
                .Where(uo => uo.ItemId == itemId)
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync();

            return openings.ToResponse();
        }
    }
}
