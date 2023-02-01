using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Get(Guid id)
        {
            UserRole? userRole = await _userRoleRepository.Get(id);

            if (userRole != null)
            {
                Ok(userRole);
            }

            return NotFound();
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            UserRole? userRole = await _userRoleRepository.GetByName(name);

            if (userRole != null)
            {
                return Ok(userRole);
            }

            return NotFound();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userRoleRepository.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRole userRole)
        {
            return Ok(await _userRoleRepository.Create(userRole));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserRole userRole)
        {
            return Ok(await _userRoleRepository.Update(userRole));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _userRoleRepository.Delete(id));
        }
    }
}
