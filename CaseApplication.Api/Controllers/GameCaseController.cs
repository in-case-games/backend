using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameCaseController : ControllerBase
    {
        public readonly IGameCaseRepository _gameCaseRepository;

        public GameCaseController(IGameCaseRepository gameCaseRepository)
        {
            _gameCaseRepository = gameCaseRepository;
        }

        [HttpGet]
        public async Task<GameCase> GetCase(Guid id)
        {
            return await _gameCaseRepository.GetCurrentCase(id);
        }

        [HttpGet("GetAllCases")]
        public async Task<IEnumerable<GameCase>> GetAllCases()
        {
            return await _gameCaseRepository.GetAllCases();
        }

        [HttpPost]
        public async Task<bool> CreateCase(GameCase gameCase)
        {
            return await _gameCaseRepository.CreateCase(gameCase);
        }

        [HttpPut]
        public async Task<bool> UpdateCase(GameCase gameCase)
        {
            return await _gameCaseRepository.UpdateCase(gameCase);
        }

        [HttpDelete]
        public async Task<bool> DeleteCase(Guid id)
        {
            return await _gameCaseRepository.DeleteCase(id);
        }
    }
}
