using Identity.BLL.Exceptions;
using Identity.BLL.Helpers;
using Identity.BLL.Interfaces;
using Identity.BLL.Models;
using Identity.DAL.Data;
using Identity.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.BLL.Services
{
    public class UserRestrictionService : IUserRestrictionService
    {
        private readonly ApplicationDbContext _context;

        public UserRestrictionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserRestrictionResponse> Get(Guid id)
        {
            UserRestriction restriction = await _context.Restrictions
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.Id == id) ??
                throw new NotFoundException("Эффект не найден");

            return restriction.ToResponse();
        }

        public Task<List<UserRestrictionResponse>> Get(Guid userId, Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserRestrictionResponse>> GetByLogin(string login)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserRestrictionResponse>> GetByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<RestrictionTypeResponse>> GetTypes()
        {
            throw new NotImplementedException();
        }

        public Task<UserRestrictionResponse> Create(UserRestrictionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserRestrictionResponse> Update(UserRestrictionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserRestrictionResponse> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
