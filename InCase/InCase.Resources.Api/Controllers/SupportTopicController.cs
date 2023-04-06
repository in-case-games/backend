using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/support-topic")]
    [ApiController]
    public class SupportTopicController : ControllerBase
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private Guid UserId => Guid
            .Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public SupportTopicController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<SupportTopic> supportTopics = await context.SupportTopics
                .AsNoTracking()
                .Where(w => w.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(supportTopics);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? supportTopic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == UserId);

            return supportTopic is null ?
                ResponseUtil.NotFound(nameof(SupportTopic)) :
                ResponseUtil.Ok(supportTopic);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}/answers")]
        public async Task<IActionResult> GetAnswers(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            bool IsExistTopic = await context.SupportTopics
                .AsNoTracking()
                .AnyAsync(a => a.Id == id && a.UserId == UserId);

            if (IsExistTopic is false)
                return ResponseUtil.NotFound(nameof(SupportTopic));

            List<SupportTopicAnswer> answers = await context.SupportTopicAnswers
                .Include(i => i.Plaintiff)
                .Include(i => i.Images)
                .AsNoTracking()
                .Where(w => w.TopicId == id)
                .ToListAsync();

            return ResponseUtil.Ok(answers);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}/answer/{answerId}")]
        public async Task<IActionResult> GetAnswer(Guid id, Guid answerId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            bool IsExistTopic = await context.SupportTopics
                .AsNoTracking()
                .AnyAsync(a => a.Id == id && a.UserId == UserId);

            if (IsExistTopic is false)
                return ResponseUtil.NotFound(nameof(SupportTopic));

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .Include(i => i.Plaintiff)
                .Include(i => i.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == answerId);

            return answer is null ?
                ResponseUtil.NotFound(nameof(SupportTopicAnswer)) :
                ResponseUtil.Ok(answer);
        }

        [AuthorizeRoles(Roles.SupportOwnerBot)]
        [HttpGet("support")]
        public async Task<IActionResult> GetTopicsBySupport()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<SupportTopic> supportTopics = await context.SupportTopics
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(supportTopics);
        }

        [AuthorizeRoles(Roles.SupportOwnerBot)]
        [HttpGet("{id}/support")]
        public async Task<IActionResult> GetTopicBySupport(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? supportTopic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return supportTopic is null ?
                ResponseUtil.NotFound(nameof(SupportTopic)) :
                ResponseUtil.Ok(supportTopic);
        }

        [AuthorizeRoles(Roles.SupportOwnerBot)]
        [HttpGet("support/answer/{id}")]
        public async Task<IActionResult> GetAnswerBySupport(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .Include(i => i.Plaintiff)
                .Include(i => i.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return answer is null ?
                ResponseUtil.NotFound(nameof(SupportTopicAnswer)) :
                ResponseUtil.Ok(answer);
        }

        [AuthorizeRoles(Roles.SupportOwnerBot)]
        [HttpGet("{id}/support/answers")]
        public async Task<IActionResult> GetAnswersBySupport(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<SupportTopicAnswer> answers = await context.SupportTopicAnswers
                .Include(i => i.Plaintiff)
                .Include(i => i.Images)
                .AsNoTracking()
                .Where(w => w.TopicId == id)
                .ToListAsync();

            return ResponseUtil.Ok(answers);
        }

        //TODO
        [AuthorizeRoles(Roles.User)]
        [HttpPost]
        public async Task<IActionResult> Create(SupportTopicDto supportTopic)
        {
            return await EndpointUtil.Create(supportTopic.Convert(), _contextFactory);
        }

        //TODO
        [AuthorizeRoles(Roles.AllExceptAdmin)]
        [HttpPost("answer")]
        public async Task<IActionResult> CreateAnswer(SupportTopicAnswerDto answerDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                if (await context.SupportTopics.AnyAsync(a => a.Id == answerDto.TopicId))
                {
                    await context.SupportTopicAnswers.AddAsync(answerDto.Convert());
                    await context.SaveChangesAsync();

                    return ResponseUtil.Ok(answerDto);
                }

                return ResponseUtil.NotFound(nameof(SupportTopic));
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }

        //TODO
        [AuthorizeRoles(Roles.AllExceptAdmin)]
        [HttpPut()]
        public async Task<IActionResult> Update(SupportTopicDto supportTopic)
        {
            return await EndpointUtil.Update(supportTopic.Convert(), _contextFactory);
        }

        //TODO
        [AuthorizeRoles(Roles.AllExceptAdmin)]
        [HttpPut("answers")]
        public async Task<IActionResult> UpdateAnswer(SupportTopicAnswerDto answer)
        {
            return await EndpointUtil.Update(answer.Convert(), _contextFactory);
        }

        [AuthorizeRoles(Roles.Owner, Roles.Bot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await EndpointUtil.Delete<SupportTopic>(id, _contextFactory);
        }

        //TODO
        [AuthorizeRoles(Roles.AllExceptAdmin)]
        [HttpDelete("answer/{id}")]
        public async Task<IActionResult> DeleteAnswer(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id && f.PlaintiffId == UserId);

            if (answer is null)
                return ResponseUtil.NotFound(nameof(SupportTopicAnswer));

            context.Set<SupportTopicAnswer>().Remove(answer);
            await context.SaveChangesAsync();

            return ResponseUtil.Delete(nameof(SupportTopicAnswer));
        }
    }
}
