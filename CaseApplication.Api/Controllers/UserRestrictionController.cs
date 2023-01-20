using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserRestrictionController : ControllerBase
    {
        private readonly IUserRestrictionRepository _userRestrictionRepository;

        public UserRestrictionController(IUserRestrictionRepository userRestrictionRepository)
        {
            _userRestrictionRepository = userRestrictionRepository;
        }

        [HttpGet]
        public async Task<UserRestriction> GetRestriction(Guid id)
        {
            return await _userRestrictionRepository.GetRestriction(id);
        }

        [HttpGet("GetAllRestrictions")]
        public async Task<IEnumerable<UserRestriction>> GetAllRestrictions(Guid userId)
        {
            return await _userRestrictionRepository.GetAllRestrictions(userId);
        }

        [HttpPost]
        public async Task<bool> CreateRestriction(UserRestriction userRestriction)
        {
            return await _userRestrictionRepository.CreateRestriction(userRestriction);
        }

        [HttpPut]
        public async Task<bool> UpdateRestriction(UserRestriction userRestriction)
        {
            return await _userRestrictionRepository.UpdateRestriction(userRestriction);
        }

        [HttpDelete]
        public async Task<bool> DeleteRestriction(Guid id)
        {
            return await _userRestrictionRepository.DeleteRestriction(id);
        }
    }
}
