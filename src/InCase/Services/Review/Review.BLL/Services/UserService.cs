﻿using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;
using Review.BLL.Exceptions;
using Review.BLL.Interfaces;
using Review.DAL.Data;
using Review.DAL.Entities;

namespace Review.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetAsync(Guid id, CancellationToken cancellation = default) => 
            await _context.User
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellation);

        public async Task CreateAsync(UserTemplate template, CancellationToken cancellation = default)
        {
            if (await _context.User.AnyAsync(u => u.Id == template.Id, cancellation))
                throw new NotFoundException("Пользователь существует");

            await _context.User.AddAsync(new User { Id = template.Id }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Id == id, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            _context.User.Remove(user);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
