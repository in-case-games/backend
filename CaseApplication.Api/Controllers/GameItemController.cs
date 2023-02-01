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
        public async Task<IActionResult> Get(Guid id)
        {
            GameItem? gameItem = await _gameItemRepository.Get(id);

            if (gameItem != null) {
                return Ok(await _gameItemRepository.Get(id));
            }

            return NotFound();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _gameItemRepository.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameItem item)
        {
            return Ok(await _gameItemRepository.Create(item));
        }

        [HttpPut]
        public async Task<IActionResult> Update(GameItem item)
        {
            return Ok(await _gameItemRepository.Update(item));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _gameItemRepository.Delete(id));
        }
    }
}
