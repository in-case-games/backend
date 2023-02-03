using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PromocodeTypeController : ControllerBase
    {
        private readonly IPromocodeTypeRepository _promocodeTypeRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PromocodeTypeController(IPromocodeTypeRepository promocodeTypeRepository)
        {
            _promocodeTypeRepository = promocodeTypeRepository;
        }

        [AllowAnonymous]
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            PromocodeType? promocodeType = await _promocodeTypeRepository.GetByName(name);

            if (promocodeType != null)
            {
                return Ok(promocodeType);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _promocodeTypeRepository.GetAll());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(PromocodeType promocodeType)
        {
            return Ok(await _promocodeTypeRepository.Create(promocodeType));
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> Update(PromocodeType promocodeType)
        {
            return Ok(await _promocodeTypeRepository.Update(promocodeType));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _promocodeTypeRepository.Delete(id));
        }
    }
}