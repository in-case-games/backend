using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PromocodeTypeController : ControllerBase
    {
        private readonly IPromocodeTypeRepository _promocodeTypeRepository;

        public PromocodeTypeController(IPromocodeTypeRepository promocodeTypeRepository)
        {
            _promocodeTypeRepository = promocodeTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            PromocodeType? promocodeType = await _promocodeTypeRepository.Get(id);

            if(promocodeType != null)
            {
                return Ok(promocodeType);
            }

            return NotFound();
        }

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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _promocodeTypeRepository.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PromocodeType promocodeType)
        {
            return Ok(await _promocodeTypeRepository.Create(promocodeType));
        }

        [HttpPut]
        public async Task<IActionResult> Update(PromocodeType promocodeType)
        {
            return Ok(await _promocodeTypeRepository.Update(promocodeType));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _promocodeTypeRepository.Delete(id));
        }
    }
}