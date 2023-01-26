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
        public async Task<News> Get(Guid id)
        {
            return await _newsRepository.Get(id);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<News>> GetAll()
        {
            return await _newsRepository.GetAll();
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
