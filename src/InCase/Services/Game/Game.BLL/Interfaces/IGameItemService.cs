using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;

namespace Game.BLL.Interfaces
{
    public interface IGameItemService
    {
        public Task<GameItem?> GetAsync(Guid id);
        public Task<GameItem> CreateAsync(GameItemTemplate template);
        public Task UpdateAsync(GameItemTemplate template);
        public Task DeleteAsync(Guid id);
    }
}
