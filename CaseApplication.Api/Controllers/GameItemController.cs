using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameItemController : ControllerBase
    {
        private readonly IGameItemRepository _gameItemRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public GameItemController(IGameItemRepository gameItemRepository)
        {
            _gameItemRepository = gameItemRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            GameItem? gameItem = await _gameItemRepository.Get(id);

            if (gameItem != null) {
                return Ok(gameItem);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _gameItemRepository.GetAll());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(GameItem item)
        {
            return Ok(await _gameItemRepository.Create(item));
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> Update(GameItem item)
        {
            return Ok(await _gameItemRepository.Update(item));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _gameItemRepository.Delete(id));
        }
    }
}
