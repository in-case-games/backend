using Game.BLL.Models;
using Game.DAL.Entities;
using Infrastructure.MassTransit.User;

namespace Game.BLL.Interfaces
{
    public interface IUserPromocodeService
    {
        public Task<UserPromocode?> GetAsync(Guid id);
        public Task CreateAsync(UserPromocodeTemplate template);
        public Task UpdateAsync(UserPromocodeTemplate rtemplate);
    }
}
