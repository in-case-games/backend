using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
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

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            PromocodesUsedByUser? promocodesUsed = await _promocodeUserByUserRepository.Get(id);

            if(promocodesUsed != null)
            {
                return Ok(promocodesUsed);
            }

            return NotFound();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid userId)
        {
            return Ok(await _promocodeUserByUserRepository.GetAll(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(PromocodesUsedByUser promocodesUsedByUser)
        {
            return Ok(await _promocodeUserByUserRepository.Create(promocodesUsedByUser));
        }

        [HttpPut]
        public async Task<IActionResult> Update(PromocodesUsedByUser promocodesUsedByUser)
        {
            return Ok(await _promocodeUserByUserRepository.Update(promocodesUsedByUser));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _promocodeUserByUserRepository.Delete(id));
        }
    }
}