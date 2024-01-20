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
    public class UserPromocodesService(ApplicationDbContext context, BasePublisher publisher) : IUserPromocodesService
    {
        public async Task<UserPromocodeResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var promocode = await context.UserPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp => uhp.Id == id, cancellation) ??
                throw new NotFoundException("История активации промокода не найдена");

            return promocode.ToResponse();
        }

        public async Task<UserPromocodeResponse> GetAsync(Guid id, Guid userId, CancellationToken cancellation = default)
        {
            var promocode = await context.UserPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp => uhp.Id == id, cancellation) ??
                throw new NotFoundException("История активации промокода не найдена");

            return promocode.UserId == userId ?
                promocode.ToResponse() :
                throw new ForbiddenException("История активации числится на другого пользователя");
        }

        public async Task<List<UserPromocodeResponse>> GetAsync(Guid userId, int count, CancellationToken cancellation = default)
        {
            if (count is <= 0 or >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            var promocode = await context.UserPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .AsNoTracking()
                .Where(uhp => uhp.UserId == userId)
                .OrderByDescending(uhp => uhp.Date)
                .Take(count)
                .ToListAsync(cancellation);

            return promocode.ToResponse();
        }

        public async Task<List<UserPromocodeResponse>> GetAsync(int count, CancellationToken cancellation = default)
        {
            if (count is <= 0 or >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            var history = await context.UserPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .AsNoTracking()
                .OrderByDescending(uhp => uhp.Date)
                .Take(count)
                .ToListAsync(cancellation);

            return history.ToResponse();
        }

        public async Task<UserPromocodeResponse> ActivateAsync(Guid userId, string name, CancellationToken cancellation = default)
        {
            var promocode = await context.Promocodes
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.Name == name, cancellation) ??
                throw new NotFoundException("Промокод не найден");

            var isUsedType = await context.UserPromocodes.AnyAsync(up =>
                up.Promocode!.Type!.Id == promocode.TypeId &&
                up.IsActivated == false &&
                up.UserId == userId, cancellation);

            if (promocode.NumberActivations <= 0 || promocode.ExpirationDate <= DateTime.UtcNow)
                throw new ForbiddenException("Промокод истёк");
            if (await context.UserPromocodes.AnyAsync(uhp => uhp.PromocodeId == promocode.Id, cancellation))
                throw new ConflictException("Промокод уже используется");
            if (isUsedType)
                throw new ConflictException("Тип промокода уже используется");

            var userPromocode = new UserPromocode
            {
                IsActivated = false,
                Date = DateTime.UtcNow,
                PromocodeId = promocode.Id,
                UserId = userId,
                Promocode = promocode,
            };

            promocode.NumberActivations--;

            await context.UserPromocodes.AddAsync(userPromocode, cancellation);
            await context.SaveChangesAsync(cancellation);
            await publisher.SendAsync(userPromocode.ToTemplate(), cancellation);

            return userPromocode.ToResponse();
        }

        public async Task<UserPromocodeResponse> ExchangeAsync(Guid userId, string name, CancellationToken cancellation = default)
        {
            var promocode = await context.Promocodes
                .Include(p => p.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == name, cancellation) ??
                throw new NotFoundException("Промокод не найден");

            if (promocode.NumberActivations <= 0 || promocode.ExpirationDate <= DateTime.UtcNow)
                throw new ConflictException("Промокод истёк");

            var userPromocode = await context.UserPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp =>
                uhp.Promocode!.Type!.Id == promocode.TypeId &&
                uhp.IsActivated == false &&
                uhp.UserId == userId, cancellation) ??
                throw new ConflictException("Промокод не активировался");

            if (userPromocode.PromocodeId == promocode.Id)
                throw new ConflictException("Промокод уже используется");

            var promocodeOld = await context.Promocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == userPromocode.PromocodeId, cancellation) ??
                throw new NotFoundException("Прошлый промокод не найден");

            promocodeOld.NumberActivations++;
            promocode.NumberActivations--;

            userPromocode.Date = DateTime.UtcNow;
            userPromocode.PromocodeId = promocode.Id;

            context.Entry(promocodeOld).Property(p => p.NumberActivations).IsModified = true;
            context.Entry(promocode).Property(p => p.NumberActivations).IsModified = true;
            context.Entry(userPromocode).Property(p => p.Date).IsModified = true;
            context.Entry(userPromocode).Property(p => p.PromocodeId).IsModified = true;
            await context.SaveChangesAsync(cancellation);

            userPromocode.Promocode = promocode;

            await publisher.SendAsync(userPromocode.ToTemplate(), cancellation);

            return userPromocode.ToResponse();
        }
    }
}
