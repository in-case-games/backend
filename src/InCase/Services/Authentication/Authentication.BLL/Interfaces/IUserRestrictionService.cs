using Authentication.BLL.Models;
using Authentication.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Authentication.BLL.Interfaces
{
    public interface IUserRestrictionService
    {
        public Task<UserRestriction?> GetAsync(Guid id);
        public Task CreateAsync(UserRestrictionTemplate template);
        public Task UpdateAsync(UserRestrictionTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
