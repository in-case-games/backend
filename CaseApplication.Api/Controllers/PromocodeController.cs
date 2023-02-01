using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PromocodeController : ControllerBase
    {
        private readonly IPromocodeRepository _promocodeRepository;

        public PromocodeController(IPromocodeRepository promocodeRepository)
        {
            _promocodeRepository = promocodeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            Promocode? promocode = await _promocodeRepository.Get(id);

            if (promocode != null)
            {
                return Ok(promocode);
            }

            return NotFound();
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            Promocode? promocode = await _promocodeRepository.GetByName(name);

            if (promocode != null)
            {
                return Ok(promocode);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Promocode promocode)
        {
            return Ok(await _promocodeRepository.Create(promocode));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Promocode promocode)
        {
            return Ok(await _promocodeRepository.Update(promocode));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _promocodeRepository.Delete(id));
        }
    }
}