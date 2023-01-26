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
        public async Task<GameItem> Get(Guid id)
        {
            return await _gameItemRepository.Get(id);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<GameItem>> GetAll()
        {
            return await _gameItemRepository.GetAll();
        }

        [HttpPost]
        public async Task<bool> Create(GameItem item)
        {
            return await _gameItemRepository.Create(item);
        }

        [HttpPut]
        public async Task<bool> Update(GameItem item)
        {
            return await _gameItemRepository.Update(item);
        }

        [HttpDelete]
        public async Task<bool> Delete(Guid id)
        {
            return await _gameItemRepository.Delete(id);
        }
    }
}
