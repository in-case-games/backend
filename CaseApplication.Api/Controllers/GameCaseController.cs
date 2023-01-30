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
        public async Task<GameCase> Get(Guid id)
        {
            return await _gameCaseRepository.Get(id);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<GameCase>> GetAll()
        {
            return await _gameCaseRepository.GetAll();
        }
        [HttpGet("GetAllByGroupName")]
        public async Task<IEnumerable<GameCase>> GetAllByGroupName(string name)
        {
            return await _gameCaseRepository.GetAllByGroupName(name);
        }

        [HttpPost]
        public async Task<bool> Create(GameCase gameCase)
        {
            return await _gameCaseRepository.Create(gameCase);
        }

        [HttpPut]
        public async Task<bool> Update(GameCase gameCase)
        {
            return await _gameCaseRepository.Update(gameCase);
        }

        [HttpDelete]
        public async Task<bool> Delete(Guid id)
        {
            return await _gameCaseRepository.Delete(id);
        }
    }
}
