using Identity.BLL.Exceptions;
using Identity.BLL.Helpers;
using Identity.BLL.Interfaces;
using Identity.BLL.MassTransit;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Identity.DAL.Entities;
using ImageMagick;
using Infrastructure.MassTransit.User;
using Microsoft.EntityFrameworkCore;

namespace Identity.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByConsumerAsync(Guid id, CancellationToken cancellation = default) => 
            await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellation);
           
        public async Task<UserResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .Include(u => u.Restrictions!)
                    .ThenInclude(r => r.Type)
                .Include(u => u.OwnerRestrictions!)
                    .ThenInclude(r => r.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            return new UserResponse()
            {
                Id = user.Id,
                AdditionalInfo = user.AdditionalInfo?.ToResponse(),
                Login = user.Login,
                Restrictions = user.Restrictions?.ToResponse(),
                OwnerRestrictions = user.OwnerRestrictions?.ToResponse(),
            };
        }

        public async Task<UserResponse> GetAsync(string login, CancellationToken cancellation = default)
        {
            var user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .Include(u => u.Restrictions!)
                    .ThenInclude(r => r.Type)
                .Include(u => u.OwnerRestrictions!)
                    .ThenInclude(r => r.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            return new UserResponse()
            {
                Id = user.Id,
                AdditionalInfo = user.AdditionalInfo?.ToResponse(),
                Login = user.Login,
                Restrictions = user.Restrictions?.ToResponse(),
                OwnerRestrictions = user.OwnerRestrictions?.ToResponse(),
            };
        }

        public async Task CreateAsync(UserTemplate template, CancellationToken cancellation = default)
        {
            if (await _context.Users.AnyAsync(u => u.Id == template.Id, cancellation))
                throw new ForbiddenException("Пользователь существует");

            var role = await _context.Roles.FirstAsync(ur => ur.Name == "user", cancellation);

            await _context.Users.AddAsync(new User
            {
                Id = template.Id,
                Login = template.Login
            }, cancellation);
            await _context.AdditionalInfos.AddAsync(new UserAdditionalInfo
            {
                UserId = template.Id,
                CreationDate = DateTime.UtcNow,
                RoleId = role.Id
            }, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellation) ??
                throw new NotFoundException("Пользователь не найден");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateLoginAsync(UserTemplate template, CancellationToken cancellation = default)
        {
            var user = await _context.Users
                 .Include(u => u.AdditionalInfo)
                 .Include(u => u.Restrictions)
                 .Include(u => u.OwnerRestrictions)
                 .FirstOrDefaultAsync(u => u.Id == template.Id, cancellation) ??
                 throw new NotFoundException("Пользователь не найден");

            user.Login = template.Login;

            await _context.SaveChangesAsync(cancellation);
        }
    }
}
