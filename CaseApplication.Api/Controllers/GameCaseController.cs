using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameCaseController : ControllerBase
    {
        private readonly IGameCaseRepository _gameCaseRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public GameCaseController(IGameCaseRepository gameCaseRepository)
        {
            _gameCaseRepository = gameCaseRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            GameCase? gameCase = await _gameCaseRepository.Get(id);

            if (gameCase != null)
            {
                gameCase.RevenuePrecentage = 0;
                gameCase.GameCaseBalance = 0;

                return Ok(gameCase);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<GameCase> gameCases = await _gameCaseRepository.GetAll();

            foreach(GameCase gameCase in gameCases)
            {
                gameCase.RevenuePrecentage = 0;
                gameCase.GameCaseBalance = 0;
            }

            return Ok(gameCases);
        }

        [AllowAnonymous]
        [HttpGet("GetAllByGroupName")]
        public async Task<IActionResult> GetAllByGroupName(string name)
        {
            List<GameCase> gameCases = await _gameCaseRepository.GetAllByGroupName(name);

            foreach (GameCase gameCase in gameCases)
            {
                gameCase.RevenuePrecentage = 0;
                gameCase.GameCaseBalance = 0;
            }

            return Ok(gameCases);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(GameCase gameCase)
        {
            return Ok(await _gameCaseRepository.Create(gameCase));
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> Update(GameCase gameCase)
        {
            return Ok(await _gameCaseRepository.Update(gameCase));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _gameCaseRepository.Delete(id));
        }
    }
}
