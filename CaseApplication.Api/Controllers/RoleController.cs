using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUserRoleRepository _userRoleRepository;
        public RoleController(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        [HttpGet]
        public async Task<UserRole> GetRole(Guid id = new(), string? roleName = null)
        {
            UserRole role = new() { Id = id, RoleName = roleName };
            return await _userRoleRepository.GetRole(role);
        }

        [HttpGet("GetAllRoles")]
        public async Task<IEnumerable<UserRole>> GetAllRoles()
        {
            return await _userRoleRepository.GetAllRoles();
        }

        [HttpPost]
        public async Task<bool> CreateRole(UserRole userRole)
        {
            return await _userRoleRepository.CreateRole(userRole);
        }

        [HttpPut]
        public async Task<bool> UpdateRole(UserRole userRole)
        {
            return await _userRoleRepository.UpdateRole(userRole);
        }

        [HttpDelete]
        public async Task<bool> DeleteRole(UserRole userRole)
        {
            return await _userRoleRepository.DeleteRole(userRole);
        }
    }
}
