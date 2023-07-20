using Infrastructure.MassTransit.Resources;
using Withdraw.DAL.Entities;

namespace Withdraw.BLL.Interfaces
{
    public interface IGameItemService
    {
        public Task<GameItem?> GetAsync(Guid id);
        public Task CreateAsync(GameItemTemplate template);
        public Task UpdateAsync(GameItemTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
