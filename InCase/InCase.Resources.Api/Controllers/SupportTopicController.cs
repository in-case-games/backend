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

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .Where(w => w.UserId == UserId)
                .ToListAsync();

            return topics.Count == 0 ?
                ResponseUtil.NotFound(nameof(SupportTopic)) :
                ResponseUtil.Ok(topics);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == UserId);

            return topic is null ?
                ResponseUtil.NotFound(nameof(SupportTopic)) :
                ResponseUtil.Ok(topic);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}/answers")]
        public async Task<IActionResult> GetAnswers(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .Include(i => i.Answers!)
                    .ThenInclude(ti => ti.Plaintiff)
                .Include(i => i.Answers!)
                    .ThenInclude(ti => ti.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == UserId);

            if (topic is null)
                return ResponseUtil.NotFound(nameof(SupportTopic));

            return topic.Answers is null || topic.Answers.Count == 0 ?
                ResponseUtil.NotFound(nameof(SupportTopicAnswer)) : 
                ResponseUtil.Ok(topic.Answers);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}/answer/{answerId}")]
        public async Task<IActionResult> GetAnswer(Guid id, Guid answerId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .Include(i => i.Answers!)
                    .ThenInclude(ti => ti.Plaintiff)
                .Include(i => i.Answers!)
                    .ThenInclude(ti => ti.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == UserId);

            if (topic is null)
                return ResponseUtil.NotFound(nameof(SupportTopic));
            if (topic.Answers is null || topic.Answers.Count == 0)
                return ResponseUtil.NotFound(nameof(SupportTopicAnswer));

            SupportTopicAnswer? answer = topic.Answers
                .FirstOrDefault(f => f.Id == answerId);

            return answer is null ?
                ResponseUtil.NotFound(nameof(SupportTopicAnswer)) :
                ResponseUtil.Ok(answer);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("support")]
        public async Task<IActionResult> GetTopicsBySupport()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .ToListAsync();

            return topics.Count == 0 ?
                ResponseUtil.NotFound(nameof(SupportTopic)) :
                ResponseUtil.Ok(topics);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("{id}/support")]
        public async Task<IActionResult> GetTopicBySupport(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            return topic is null ?
                ResponseUtil.NotFound(nameof(SupportTopic)) :
                ResponseUtil.Ok(topic);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
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

        [AuthorizeRoles(Roles.AdminOwnerBot)]
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

            return answers.Count == 0 ?
                ResponseUtil.NotFound(nameof(SupportTopicAnswer)) :
                ResponseUtil.Ok(answers);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPost]
        public async Task<IActionResult> Create(SupportTopicDto topicDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            topicDto.UserId = UserId;

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .Where(w => w.UserId == topicDto.UserId)
                .ToListAsync();

            if (topics.Count == 3)
                return ResponseUtil.Conflict("Access denied");

            return await EndpointUtil.Create(topicDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPost("answer")]
        public async Task<IActionResult> CreateAnswer(SupportTopicAnswerDto answerDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .FirstOrDefaultAsync(f => f.Id == answerDto.TopicId && f.UserId == UserId);

            if (topic is null)
                return ResponseUtil.NotFound(nameof(SupportTopic));

            answerDto.PlaintiffId = UserId;

            return await EndpointUtil.Create(answerDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPost("support/answer")]
        public async Task<IActionResult> CreateAnswerBySupport(SupportTopicAnswerDto answerDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == answerDto.TopicId);

            if (topic is null)
                return ResponseUtil.NotFound(nameof(SupportTopic));

            answerDto.PlaintiffId = UserId;

            return await EndpointUtil.Create(answerDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.UserAdminOwner)]
        [HttpPut]
        public async Task<IActionResult> Update(SupportTopicDto topicDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .FirstOrDefaultAsync(f => f.Id == topicDto.Id && f.UserId == UserId);

            if (topic is null)
                return ResponseUtil.NotFound(nameof(SupportTopic));

            topicDto.UserId = topic.UserId;

            return await EndpointUtil.Update(topic, topicDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.UserAdminOwner)]
        [HttpPut("answer")]
        public async Task<IActionResult> UpdateAnswer(SupportTopicAnswerDto answerDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .FirstOrDefaultAsync(f => f.Id == answerDto.Id && f.PlaintiffId == UserId);

            if (answer is null)
                return ResponseUtil.NotFound(nameof(SupportTopicAnswer));

            answerDto.PlaintiffId = UserId;

            return await EndpointUtil.Update(answer, answerDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Delete<SupportTopic>(id, context);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpDelete("answer/{id}")]
        public async Task<IActionResult> DeleteAnswer(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id && f.PlaintiffId == UserId);

            if (answer is null)
                return ResponseUtil.NotFound(nameof(SupportTopicAnswer));

            return await EndpointUtil.Delete(answer, context);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpDelete("support/answer/{id}")]
        public async Task<IActionResult> DeleteAnswerBySupport(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            return await EndpointUtil.Delete<SupportTopicAnswer>(id, context);
        }
    }
}
