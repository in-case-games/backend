using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
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
        public async Task<UserRestriction> Get(Guid id)
        {
            return await _userRestrictionRepository.Get(id);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<UserRestriction>> GetAll(Guid userId)
        {
            return await _userRestrictionRepository.GetAll(userId);
        }

        [HttpPost]
        public async Task<bool> Create(UserRestriction userRestriction)
        {
            return await _userRestrictionRepository.Create(userRestriction);
        }

        [HttpPut]
        public async Task<bool> Update(UserRestriction userRestriction)
        {
            return await _userRestrictionRepository.Update(userRestriction);
        }

        [HttpDelete]
        public async Task<bool> Delete(Guid id)
        {
            return await _userRestrictionRepository.Delete(id);
        }
    }
}
