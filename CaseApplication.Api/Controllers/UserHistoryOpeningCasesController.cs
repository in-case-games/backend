using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserHistoryOpeningCasesController : ControllerBase
    {
        private readonly IUserHistoryOpeningCasesRepository _userHistoryRepository;

        public UserHistoryOpeningCasesController(IUserHistoryOpeningCasesRepository userHistoryRepository)
        {
            _userHistoryRepository = userHistoryRepository;
        }

        [HttpGet]
        public async Task<UserHistoryOpeningCases> GetUserHistory(Guid id)
        {
            return await _userHistoryRepository.GetUserHistory(id);
        }

        [HttpGet("GetAllUserHistories")]
        public async Task<IEnumerable<UserHistoryOpeningCases>> GetAllUserHistories(Guid userId)
        {
            return await _userHistoryRepository.GetAllUserHistories(userId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserHistory(UserHistoryOpeningCases userHistory)
        {
            return Ok(await _userHistoryRepository.CreateUserHisory(userHistory));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserHistory(UserHistoryOpeningCases userHistory)
        {
            return Ok(await _userHistoryRepository.UpdateUserHistory(userHistory));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserHistory(Guid id)
        {
            return Ok(await _userHistoryRepository.DeleteUserHistory(id));
        }

        [HttpDelete("DeleteAllUserHistories")]
        public async Task<IActionResult> DeleteAllUserHistories(Guid userId)
        {
            return Ok(await _userHistoryRepository.DeleteAllUserHistories(userId));
        }
    }
}
