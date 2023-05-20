﻿using Microsoft.EntityFrameworkCore;
using Promocode.BLL.Exceptions;
using Promocode.BLL.Helpers;
using Promocode.BLL.Interfaces;
using Promocode.BLL.Models;
using Promocode.DAL.Data;
using Promocode.DAL.Entities;

namespace Promocode.BLL.Services
{
    public class UserPromocodesService : IUserPromocodesService
    {
        private readonly ApplicationDbContext _context;

        public UserPromocodesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserHistoryPromocodeResponse> GetAsync(Guid id, Guid userId)
        {
            UserHistoryPromocode history = await _context.UserHistoriesPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp => uhp.Id == id) ??
                throw new NotFoundException("История активации промокода не найдена");

            if (history.UserId != userId)
                throw new ForbiddenException("История активации числится на другого пользователя");

            return history.ToResponse();
        }

        public async Task<List<UserHistoryPromocodeResponse>> GetAsync(Guid userId, int count)
        {
            if (count <= 0 || count >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            List<UserHistoryPromocode> history = await _context.UserHistoriesPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .AsNoTracking()
                .Where(uhp => uhp.UserId == userId)
                .OrderByDescending(uhp => uhp.Date)
                .Take(count)
                .ToListAsync();

            return history.ToResponse();
        }

        public async Task<List<UserHistoryPromocodeResponse>> GetAsync(int count)
        {
            if (count <= 0 || count >= 10000)
                throw new BadRequestException("Размер выборки должен быть в пределе 1-10000");

            List<UserHistoryPromocode> history = await _context.UserHistoriesPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .AsNoTracking()
                .OrderByDescending(uhp => uhp.Date)
                .Take(count)
                .ToListAsync();

            return history.ToResponse();
        }

        public async Task<UserHistoryPromocodeResponse> ActivateAsync(Guid userId, string name)
        {
            PromocodeEntity promocode = await _context.Promocodes
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.Name == name) ??
                throw new NotFoundException("Промокод не найден");

            if (promocode.NumberActivations <= 0 || promocode.ExpirationDate <= DateTime.UtcNow)
                throw new ForbiddenException("Промокод истёк");

            UserHistoryPromocode? history = await _context.UserHistoriesPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp => uhp.PromocodeId == promocode.Id);

            UserHistoryPromocode? historyType = await _context.UserHistoriesPromocodes
                .AsNoTracking()
                .FirstOrDefaultAsync(uhp =>
                uhp.Promocode!.Type!.Id == promocode.TypeId &&
                uhp.IsActivated == false &&
                uhp.UserId == userId);

            if (history is not null && history.IsActivated)
                throw new ConflictException("Промокод уже используется");
            if (historyType is not null)
                throw new ConflictException("Тип промокода уже используется");

            promocode.NumberActivations--;

            history = new()
            {
                IsActivated = false,
                Date = DateTime.UtcNow,
                PromocodeId = promocode.Id,
                UserId = userId,
            };

            //TODO Notify rabbit mq

            await _context.UserHistoriesPromocodes.AddAsync(history);
            await _context.SaveChangesAsync();

            return history.ToResponse();
        }

        public async Task<UserHistoryPromocodeResponse> ExchangeAsync(Guid userId, string name)
        {
            PromocodeEntity promocode = await _context.Promocodes
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.Name == name) ??
                throw new NotFoundException("Промокод не найден");

            if (promocode.NumberActivations <= 0 || promocode.ExpirationDate <= DateTime.UtcNow)
                throw new ConflictException("Промокод истёк");

            bool isUsed = await _context.UserHistoriesPromocodes
                .AnyAsync(uhp => uhp.PromocodeId == promocode.Id && uhp.IsActivated);

            if (isUsed)
                throw new ConflictException("Промокод уже использован");

            UserHistoryPromocode? history = await _context.UserHistoriesPromocodes
                .Include(uhp => uhp.Promocode)
                .Include(uhp => uhp.Promocode!.Type)
                .FirstOrDefaultAsync(uhp =>
                uhp.Promocode!.Type!.Id == promocode.TypeId &&
                uhp.IsActivated == false &&
                uhp.UserId == userId) ?? 
                throw new ConflictException("Прошлый промокод не найден");

            if (history.Promocode!.Id == promocode.Id)
                throw new ConflictException("Промокод уже используется");

            //TODO Notify rabbit mq

            history.Promocode.NumberActivations++;
            history.Date = DateTime.UtcNow;
            history.PromocodeId = promocode.Id;
            promocode.NumberActivations--;

            await _context.SaveChangesAsync();

            return history.ToResponse();
        }
    }
}