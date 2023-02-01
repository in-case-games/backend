using CaseApplication.DomainLayer.Entities;
using CaseApplication.DomainLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaseApplication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            News? news = await _newsRepository.Get(id);

            if(news is not null)
            {
                return Ok(news);
            }

            return NotFound();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _newsRepository.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create(News news)
        {
            return Ok(await _newsRepository.Create(news));
        }

        [HttpPut]
        public async Task<IActionResult> Update(News news)
        {
            return Ok(await _newsRepository.Update(news));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _newsRepository.Delete(id));
        }
    }
}
