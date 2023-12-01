﻿using EmailSender.BLL.Exceptions;
using EmailSender.BLL.Helpers;
using EmailSender.BLL.Interfaces;
using EmailSender.DAL.Data;
using EmailSender.DAL.Entities;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default) => await _context.Users
            .Include(u => u.AdditionalInfo)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        public async Task<User?> GetAsync(string email, CancellationToken cancellationToken = default) => await _context.Users
            .Include(u => u.AdditionalInfo)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public async Task CreateAsync(UserTemplate template, CancellationToken cancellationToken = default)
        {
            if (await _context.Users.AnyAsync(u => u.Id == template.Id || u.Email == template.Email, cancellationToken))
                throw new ForbiddenException("Пользователь существует");

            User user = template.ToEntity();

            UserAdditionalInfo info = new()
            {
                IsNotifyEmail = true,
                UserId = template.Id,
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.AdditionalInfos.AddAsync(info, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UserTemplate template, CancellationToken cancellationToken = default)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == template.Id, cancellationToken) ??
                throw new NotFoundException("Пользователь не найден");

            user.Email = template.Email;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken) ??
                throw new NotFoundException("Пользователь не найден");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
