using Microsoft.EntityFrameworkCore;
using Promocode.BLL.Exceptions;
using Promocode.BLL.Helpers;
using Promocode.BLL.Interfaces;
using Promocode.BLL.MassTransit;
using Promocode.BLL.Models;
using Promocode.DAL.Data;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Services
{
    public class UserPromocodesService : IUserPromocodesService
    {
        private readonly ApplicationDbContext _context;
        private readonly BasePublisher _publisher;

        public UserPromocodesService(ApplicationDbContext context, BasePublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<UserPromocodeResponse> GetAsync(Guid id)
        {
            UserPromocode promocode = await _context.UserPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp => uhp.Id == id) ??
                throw new NotFoundException("История активации промокода не найдена");

            return promocode.ToResponse();
        }

        public async Task<UserPromocodeResponse> GetAsync(Guid id, Guid userId)
        {
            UserPromocode promocode = await _context.UserPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp => uhp.Id == id) ??
                throw new NotFoundException("История активации промокода не найдена");

            return promocode.UserId == userId ?
                promocode.ToResponse() :
                throw new ForbiddenException("История активации числится на другого пользователя");
        }

        public async Task<List<UserPromocodeResponse>> GetAsync(Guid userId, int count)
        {
            if (count <= 0 || count >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            List<UserPromocode> promocode = await _context.UserPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .AsNoTracking()
                .Where(uhp => uhp.UserId == userId)
                .OrderByDescending(uhp => uhp.Date)
                .Take(count)
                .ToListAsync();

            return promocode.ToResponse();
        }

        public async Task<List<UserPromocodeResponse>> GetAsync(int count)
        {
            if (count <= 0 || count >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            List<UserPromocode> history = await _context.UserPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .AsNoTracking()
                .OrderByDescending(uhp => uhp.Date)
                .Take(count)
                .ToListAsync();

            return history.ToResponse();
        }

        public async Task<UserPromocodeResponse> ActivateAsync(Guid userId, string name)
        {
            PromocodeEntity promocode = await _context.Promocodes
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.Name == name) ??
                throw new NotFoundException("Промокод не найден");

            bool isUsedType = await _context.UserPromocodes
                .AnyAsync(up =>
                up.Promocode!.Type!.Id == promocode.TypeId &&
                up.IsActivated == false &&
                up.UserId == userId);

            if (promocode.NumberActivations <= 0 || promocode.ExpirationDate <= DateTime.UtcNow)
                throw new ForbiddenException("Промокод истёк");
            if (await _context.UserPromocodes.AnyAsync(uhp => uhp.PromocodeId == promocode.Id))
                throw new ConflictException("Промокод уже используется");
            if (isUsedType)
                throw new ConflictException("Тип промокода уже используется");

            UserPromocode userPromocode = new()
            {
                IsActivated = false,
                Date = DateTime.UtcNow,
                PromocodeId = promocode.Id,
                UserId = userId,
                Promocode = promocode,
            };

            promocode.NumberActivations--;

            await _context.UserPromocodes.AddAsync(userPromocode);
            await _context.SaveChangesAsync();

            await _publisher.SendAsync(userPromocode.ToTemplate());

            return userPromocode.ToResponse();
        }

        public async Task<UserPromocodeResponse> ExchangeAsync(Guid userId, string name)
        {
            PromocodeEntity promocode = await _context.Promocodes
                .Include(p => p.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == name) ??
                throw new NotFoundException("Промокод не найден");

            if (promocode.NumberActivations <= 0 || promocode.ExpirationDate <= DateTime.UtcNow)
                throw new ConflictException("Промокод истёк");

            UserPromocode userPromocode = await _context.UserPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp =>
                uhp.Promocode!.Type!.Id == promocode.TypeId &&
                uhp.IsActivated == false &&
                uhp.UserId == userId) ??
                throw new ConflictException("Промокод не активировался");

            if (userPromocode.PromocodeId == promocode.Id)
                throw new ConflictException("Промокод уже используется");

            PromocodeEntity promocodeOld = await _context.Promocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == userPromocode.PromocodeId) ??
                throw new NotFoundException("Прошлый промокод не найден");

            promocodeOld.NumberActivations++;
            promocode.NumberActivations--;

            userPromocode.Date = DateTime.UtcNow;
            userPromocode.PromocodeId = promocode.Id;

            _context.Entry(promocodeOld).Property(p => p.NumberActivations).IsModified = true;
            _context.Entry(promocode).Property(p => p.NumberActivations).IsModified = true;
            _context.Entry(userPromocode).Property(p => p.Date).IsModified = true;
            _context.Entry(userPromocode).Property(p => p.PromocodeId).IsModified = true;

            await _context.SaveChangesAsync();

            userPromocode.Promocode = promocode;

            await _publisher.SendAsync(userPromocode.ToTemplate());

            return userPromocode.ToResponse();
        }
    }
}
