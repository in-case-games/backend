using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameCaseController : ControllerBase
    {
        private readonly IGameCaseRepository _gameCaseRepository;

        public GameCaseController(IGameCaseRepository gameCaseRepository)
        {
            _gameCaseRepository = gameCaseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            GameCase? gameCase = await _gameCaseRepository.Get(id);

            if (gameCase != null)
            {
                return Ok(await _gameCaseRepository.Get(id));
            }
            return NotFound();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _gameCaseRepository.GetAll());
        }

        [HttpGet("GetAllByGroupName")]
        public async Task<IActionResult> GetAllByGroupName(string name)
        {
            return Ok(await _gameCaseRepository.GetAllByGroupName(name));
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameCase gameCase)
        {
            return Ok(await _gameCaseRepository.Create(gameCase));
        }

        [HttpPut]
        public async Task<IActionResult> Update(GameCase gameCase)
        {
            return Ok(await _gameCaseRepository.Update(gameCase));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _gameCaseRepository.Delete(id));
        }
    }
}
