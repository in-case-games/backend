using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.BLL.Models;
using Game.DAL.Data;
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

        public async Task<UserOpeningResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var opening = await _context.Openings
                .AsNoTracking()
                .FirstOrDefaultAsync(uo => uo.Id == id, cancellation) ?? 
                throw new NotFoundException("История открытия не найдена");

            return new UserOpeningResponse
            {
                BoxId = opening.BoxId,
                Date = opening.Date,
                Id = opening.Id,
                ItemId = opening.ItemId,
                UserId = opening.UserId,
            };
        }

        public async Task<List<UserOpeningResponse>> GetAsync(int count, CancellationToken cancellation = default)
        {
            var openings = await _context.Openings
                .AsNoTracking()
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync(cancellation);

            return openings.ToResponse();
        }

        public async Task<List<UserOpeningResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId, cancellation))
                throw new NotFoundException("Пользователь не найден");

            var openings = await _context.Openings
                .AsNoTracking()
                .Where(uo => uo.UserId == userId)
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync(cancellation);

            return openings.ToResponse();
        }

        public async Task<List<UserOpeningResponse>> GetByBoxIdAsync(Guid userId, Guid boxId, int count, CancellationToken cancellation = default)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId, cancellation))
                throw new NotFoundException("Пользователь не найден");
            if (!await _context.Boxes.AnyAsync(lb => lb.Id == boxId, cancellation))
                throw new NotFoundException("Кейс не найден");

            var openings = await _context.Openings
                .AsNoTracking()
                .Where(uo => uo.UserId == userId && uo.BoxId == boxId)
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync(cancellation);

            return openings.ToResponse();
        }

        public async Task<List<UserOpeningResponse>> GetByBoxIdAsync(Guid boxId, int count, CancellationToken cancellation = default)
        {
            if (!await _context.Boxes.AnyAsync(lb => lb.Id == boxId, cancellation))
                throw new NotFoundException("Кейс не найден");

            var openings = await _context.Openings
                .AsNoTracking()
                .Where(uo => uo.BoxId == boxId)
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync(cancellation);

            return openings.ToResponse();
        }

        public async Task<List<UserOpeningResponse>> GetByItemIdAsync(Guid userId, Guid itemId, int count, CancellationToken cancellation = default)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId, cancellation))
                throw new NotFoundException("Пользователь не найден");
            if (!await _context.Items.AnyAsync(gi => gi.Id == itemId, cancellation))
                throw new NotFoundException("Предмет не найден");

            var openings = await _context.Openings
                .AsNoTracking()
                .Where(uo => uo.UserId == userId && uo.ItemId == itemId)
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync(cancellation);

            return openings.ToResponse();
        }

        public async Task<List<UserOpeningResponse>> GetByItemIdAsync(Guid itemId, int count, CancellationToken cancellation = default)
        {
            if (!await _context.Items.AnyAsync(gi => gi.Id == itemId, cancellation))
                throw new NotFoundException("Предмет не найден");

            var openings = await _context.Openings
                .AsNoTracking()
                .Where(uo => uo.ItemId == itemId)
                .OrderByDescending(uo => uo.Date)
                .Take(count)
                .ToListAsync(cancellation);

            return openings.ToResponse();
        }
    }
}
