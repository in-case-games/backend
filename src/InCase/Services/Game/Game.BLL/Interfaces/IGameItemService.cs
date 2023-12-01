using Game.DAL.Entities;
using Infrastructure.MassTransit.Resources;

namespace Game.BLL.Interfaces
{
    public interface IGameItemService
    {
        public Task<GameItem?> GetAsync(Guid id, CancellationToken cancellation = default);
        public Task CreateAsync(GameItemTemplate template, CancellationToken cancellation = default);
        public Task UpdateAsync(GameItemTemplate template, CancellationToken cancellation = default);
        public Task DeleteAsync(Guid id, CancellationToken cancellation = default);
    }
}
