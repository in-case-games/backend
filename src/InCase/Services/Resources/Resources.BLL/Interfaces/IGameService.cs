using Resources.BLL.Models;

namespace Resources.BLL.Interfaces
{
    public interface IGameService
    {
        public Task<List<GameResponse>> GetAsync();
        public Task<GameResponse> GetAsync(Guid id);
        public Task<GameResponse> GetAsync(string name);
    }
}
