using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers 
{
    [Route("[controller]")]
    [ApiController]
    public class PromocodesUsedByUserController : ControllerBase
    {
        private readonly IPromocodeUsedByUserRepository _promocodeUserByUserRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PromocodesUsedByUserController(IPromocodeUsedByUserRepository promocodeUserByUserRepository)
        {
            _promocodeUserByUserRepository = promocodeUserByUserRepository;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            PromocodesUsedByUser? promocodesUsed = await _promocodeUserByUserRepository.Get(id);

            if(promocodesUsed != null)
            {
                return Ok(promocodesUsed);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetUsedPromocodes(Guid userId)
        {
            return Ok(await _promocodeUserByUserRepository.GetAll(userId));
        }
    }
}