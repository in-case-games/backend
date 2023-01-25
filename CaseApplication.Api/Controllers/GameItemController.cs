using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameItemController : ControllerBase
    {
        private readonly IGameItemRepository _gameItemRepository;

        public GameItemController(IGameItemRepository gameItemRepository)
        {
            _gameItemRepository = gameItemRepository;
        }

        [HttpGet]
        public async Task<GameItem> GetItem(Guid id)
        {
            return await _gameItemRepository.GetCurrentItem(id);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<GameItem>> GetAllItems()
        {
            return await _gameItemRepository.GetAllItems();
        }

        [HttpPost]
        public async Task<bool> CreateItem(GameItem item)
        {
            return await _gameItemRepository.CreateItem(item);
        }

        [HttpPut]
        public async Task<bool> UpdateItem(GameItem item)
        {
            return await _gameItemRepository.UpdateItem(item);
        }

        [HttpDelete]
        public async Task<bool> DeleteItem(Guid id)
        {
            return await _gameItemRepository.DeleteItem(id);
        }
    }
}
