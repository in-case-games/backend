using Game.BLL.Models;

namespace Game.BLL.Interfaces
{
    public interface ILootBoxOpeningService
    {
        public Task<GameItemResponse> OpenBox(Guid userId, Guid id);
        public Task<GameItemResponse> OpenVirtualBox(Guid userId, Guid id);
    }
}
