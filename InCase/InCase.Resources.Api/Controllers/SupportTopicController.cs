using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportTopicController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public SupportTopicController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IEnumerable<SupportTopic> supportTopics = await context.SupportTopics
                .Include(x => x.Answers)
                .Include(x => x.User)
                .ToListAsync();

            return Ok(new { Data = supportTopics, Success = true });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? supportTopic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supportTopic is null)
                return NotFound(new { Data = "Object is not found", Success = true });

            return Ok(new { Data = supportTopic, Success = true });
        }
        [HttpGet("{id}/answers")]
        public async Task<IActionResult> GetAnswers(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IEnumerable<SupportTopicAnswer> answers = await context.SupportTopicAnswers
                .Include(x => x.Plaintiff)
                .Where(x => x.TopicId == id)
                .Include(x => x.Images)
                .ToListAsync();

            return Ok(new { Data = answers, Success = true });
        }
        [HttpGet("{id}/answers/{answerId}")]
        public async Task<IActionResult> GetAnswer(Guid id, Guid answerId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .Include(x => x.Plaintiff)
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.TopicId == id && x.Id == answerId);

            if (answer is null)
                return NotFound(new { Data = "Object is not found", Success = false });

            return Ok(new { Data = answer, Success = true });
        }
        [HttpPost]
        public async Task<IActionResult> Create(SupportTopicDto supportTopic)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.SupportTopics.AddAsync(supportTopic.Convert());
                await context.SaveChangesAsync();

                return Ok(new { Data = supportTopic, Success = true });
            }
            catch (Exception ex)
            {
                return Conflict(new { Data = ex.InnerException!.Message.ToString(), Success = false });
            }
        }
        [HttpPost("{id}/answers")]
        public async Task<IActionResult> CreateAnswer(Guid id, SupportTopicAnswerDto supportTopicAnswer)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                if (await context.SupportTopics
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id) is null)
                {
                    return NotFound(
                        new
                        {
                            Data = "Object (SupportTopic) is not found. [parameter: id]",
                            Success = true
                        });
                }

                supportTopicAnswer.TopicId = id;

                await context.SupportTopicAnswers.AddAsync(supportTopicAnswer.Convert());
                await context.SaveChangesAsync();

                return Ok(new { Data = supportTopicAnswer, Success = true });
            }
            catch (Exception ex)
            {
                return Conflict(new { Data = ex.InnerException!.Message.ToString(), Success = false }); ;
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, SupportTopicDto supportTopic)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                supportTopic.Id = id;
                context.SupportTopics.Update(supportTopic.Convert());
                await context.SaveChangesAsync();

                return Ok(new { Data = supportTopic, Success = true });
            }
            catch (Exception ex)
            {
                return Conflict(new { Data = ex.InnerException!.Message.ToString(), Success = false });
            }
        }
        [HttpPut("{id}/answers/{answerId}")]
        public async Task<IActionResult> UpdateAnswer(Guid id, Guid answerId, SupportTopicAnswerDto answer)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                if (await context.SupportTopics
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id) is null)
                {
                    return NotFound(
                        new
                        {
                            Data = "Object (SupportTopic) is not found. [parameter: id]",
                            Success = true
                        });
                }

                answer.Id = id;
                context.SupportTopicAnswers.Update(answer.Convert());
                await context.SaveChangesAsync();

                return Ok(new { Data = answer, Success = true });
            }
            catch (Exception ex)
            {
                return Conflict(new { Data = ex.InnerException!.Message.ToString(), Success = false });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? supportTopic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supportTopic is null)
                return NotFound(new { Data = "Object is not found", Success = false });

            context.SupportTopics.Remove(supportTopic);
            await context.SaveChangesAsync();

            return Accepted(new { Data = "Object was successfully removed", Success = true });
        }
    }
}
