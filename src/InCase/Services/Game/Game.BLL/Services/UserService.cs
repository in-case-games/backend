using Game.BLL.Exceptions;
using Game.BLL.Helpers;
using Game.BLL.Interfaces;
using Game.DAL.Data;
using Game.DAL.Entities;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace Game.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetAsync(Guid id, CancellationToken cancellation = default) => await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellation);

        public async Task CreateAsync(UserTemplate template, CancellationToken cancellation = default)
        {
            if (await _context.Users.AnyAsync(u => u.Id == template.Id, cancellation))
                throw new NotFoundException("Пользователь существует");

            User user = template.ToEntity();

            UserAdditionalInfo info = new()
            {
                UserId = template.Id,
                Balance = 0,
                IsGuestMode = false,
            };

            await _context.Users.AddAsync(user, cancellation);
            await _context.AdditionalInfos.AddAsync(info, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
