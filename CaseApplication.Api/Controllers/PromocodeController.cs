using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PromocodeController : ControllerBase
    {
        private readonly IPromocodeRepository _promocodeRepository;
        private readonly IPromocodeUsedByUserRepository _promocodeUsedRepository;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public PromocodeController(
            IPromocodeRepository promocodeRepository, 
            IPromocodeUsedByUserRepository promocodeUsedRepository)
        {
            _promocodeRepository = promocodeRepository;
            _promocodeUsedRepository = promocodeUsedRepository;
        }

        [Authorize]
        [HttpPost("UsePromocode")]
        public async Task<IActionResult> UsePromocode(Promocode promocode)
        {
            Promocode? searchPromocode = await _promocodeRepository.GetByName(promocode.PromocodeName!);

            if (promocode == null) return NotFound();

            List<PromocodesUsedByUser> promocodesUsed = await _promocodeUsedRepository
                .GetAll(UserId);

            bool isExistPromocode = promocodesUsed.Exists(x => x.PromocodeId == promocode.Id);

            if (isExistPromocode)
                return UnprocessableEntity("Promocode is used");

            await _promocodeUsedRepository.Create(new PromocodesUsedByUser()
            {
                UserId = UserId,
                PromocodeId = promocode.Id
            });

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            Promocode? promocode = await _promocodeRepository.GetByName(name);

            if (promocode == null)
                return NotFound();

            return Ok(promocode);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Promocode promocode)
        {
            return Ok(await _promocodeRepository.Create(promocode));
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> Update(Promocode promocode)
        {
            return Ok(await _promocodeRepository.Update(promocode));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _promocodeRepository.Delete(id));
        }
    }
}