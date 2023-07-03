using Identity.BLL.Exceptions;
using Identity.BLL.Helpers;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Identity.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Identity.BLL.Services
{
    public class UserRestrictionService : IUserRestrictionService
    {
        private readonly ApplicationDbContext _context;

        public UserRestrictionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserRestrictionResponse> GetAsync(Guid id)
        {
            UserRestriction restriction = await _context.Restrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Эффект не найден");

            return restriction.ToResponse();
        }

        public async Task<List<UserRestrictionResponse>> GetAsync(Guid userId, Guid ownerId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Обвиняемый не найден");
            if (!await _context.Users.AnyAsync(u => u.Id == ownerId))
                throw new NotFoundException("Обвинитель не найден");

            List<UserRestriction> restrictions = await _context.Restrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.OwnerId == ownerId && ur.UserId == userId)
                .ToListAsync();

            return restrictions.ToResponse();
        }

        public async Task<List<UserRestrictionResponse>> GetByLoginAsync(string login)
        {
            User user = await _context.Users
                .Include(u => u.Restrictions)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login) ??
                throw new NotFoundException("Пользователь не найден");

            return user.Restrictions?.ToResponse() ?? new();
        }

        public async Task<List<UserRestrictionResponse>> GetByUserIdAsync(Guid userId)
        {
            User user = await _context.Users
                .Include(u => u.Restrictions)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId) ??
                throw new NotFoundException("Пользователь не найден");

            return user.Restrictions?.ToResponse() ?? new();
        }

        public async Task<List<UserRestrictionResponse>> GetByOwnerIdAsync(Guid ownerId)
        {
            User user = await _context.Users
                .Include(u => u.OwnerRestrictions)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == ownerId) ??
                throw new NotFoundException("Пользователь не найден");

            return user.OwnerRestrictions?.ToResponse() ?? new();
        }

        public async Task<List<RestrictionTypeResponse>> GetTypesAsync()
        {
            List<RestrictionType> types = await _context.RestrictionTypes
                .AsNoTracking()
                .ToListAsync();

            return types.ToResponse();
        }

        public async Task<UserRestrictionResponse> CreateAsync(UserRestrictionRequest request)
        {
            RestrictionType type = await _context.RestrictionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.Id == request.TypeId) ??
                throw new NotFoundException("Тип эффекта не найден");

            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.UserId) ??
                throw new NotFoundException("Обвиняемый не найден");
            User owner = await _context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.OwnerId) ??
                throw new NotFoundException("Обвинитель не найден");

            string userRole = user.AdditionalInfo!.Role!.Name!;

            if (userRole != "user")
                throw new ForbiddenException("Эффект можно наложить только на пользователя");

            request = await CheckUserRestriction(request, type);

            UserRestriction restriction = request.ToEntity(IsNewGuid: true);

            await _context.Restrictions.AddAsync(restriction);
            await _context.SaveChangesAsync();

            //TODO Notify rabbit mq auth service

            restriction.Type = type;

            return restriction.ToResponse();
        }

        public async Task<UserRestrictionResponse> UpdateAsync(UserRestrictionRequest request)
        {
            RestrictionType type = await _context.RestrictionTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.Id == request.TypeId) ??
                throw new NotFoundException("Тип эффекта не найден");

            User user = await _context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.UserId) ??
                throw new NotFoundException("Обвиняемый не найден");
            User owner = await _context.Users
                .Include(u => u.AdditionalInfo)
                .Include(u => u.AdditionalInfo!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.OwnerId) ??
                throw new NotFoundException("Обвинитель не найден");

            UserRestriction restrictionOld = await _context.Restrictions
                .Include(ur => ur.Type)
                .FirstOrDefaultAsync(ur => ur.Id == request.Id) ??
                throw new NotFoundException("Эффект не найден");

            string userRole = user.AdditionalInfo!.Role!.Name!;

            if (userRole != "user")
                throw new ForbiddenException("Эффект можно наложить только на пользователя");

            request = await CheckUserRestriction(request, type);

            UserRestriction restriction = request.ToEntity(IsNewGuid: false);

            _context.Entry(restrictionOld).CurrentValues.SetValues(restriction);
            await _context.SaveChangesAsync();

            //TODO Notify rabbit mq auth service

            restriction.Type = type;

            return restriction.ToResponse();
        }

        public async Task<UserRestrictionResponse> DeleteAsync(Guid id)
        {
            UserRestriction restriction = await _context.Restrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Эффект не найден");

            _context.Restrictions.Remove(restriction);
            await _context.SaveChangesAsync();

            //TODO Notify rabbit mq auth service

            return restriction.ToResponse();
        }

        private async Task<UserRestrictionRequest> CheckUserRestriction(
            UserRestrictionRequest request,
            RestrictionType type)
        {
            List<UserRestriction> restrictions = await _context.Restrictions
                .Include(ur => ur.Type)
                .AsNoTracking()
                .Where(ur => ur.UserId == request.UserId)
                .ToListAsync();

            int numberWarns = (type.Name == "warn") ? 1 : 0;

            foreach (var restriction in restrictions)
            {
                if (restriction.Type!.Name == "warn" && restriction.Id != request.Id)
                    ++numberWarns;
            }

            if (numberWarns >= 3)
            {
                RestrictionType? ban = await _context.RestrictionTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(rt => rt.Name == "ban");

                request.TypeId = ban!.Id;
                request.ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(30);
            }

            return request;
        }
    }
}
