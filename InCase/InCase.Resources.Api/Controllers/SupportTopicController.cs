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
                .Where(st => st.UserId == UserId)
                .ToListAsync();

            return ResponseUtil.Ok(topics);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id && st.UserId == UserId);

            return topic is null ?
                ResponseUtil.NotFound("Топик не найден") :
                ResponseUtil.Ok(topic);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}/answers")]
        public async Task<IActionResult> GetAnswers(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (topic is null)
                return ResponseUtil.NotFound("Топик не найден");
            if (topic.UserId != UserId)
                return ResponseUtil.Forbidden("Только создатель топика может его просматривать");

            List<SupportTopicAnswer> answers = await context.SupportTopicAnswers
                .Include(sta => sta.Plaintiff)
                .Include(sta => sta.Images)
                .AsNoTracking()
                .Where(sta => sta.TopicId == id)
                .ToListAsync();

            return ResponseUtil.Ok(topic.Answers);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}/answer/{answerId}")]
        public async Task<IActionResult> GetAnswer(Guid id, Guid answerId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (topic is null)
                return ResponseUtil.NotFound("Топик не найден");
            if (topic.UserId != UserId)
                return ResponseUtil.Forbidden("Только создатель топика может его просматривать");

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .Include(sta => sta.Plaintiff)
                .Include(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == answerId);

            return answer is null ?
                ResponseUtil.NotFound(nameof(SupportTopicAnswer)) :
                ResponseUtil.Ok(answer);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("support/opened")]
        public async Task<IActionResult> GetTopicsOpenedBySupport()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .Where(st => st.IsClosed == false)
                .ToListAsync();

            return ResponseUtil.Ok(topics);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("support/{timeStart}&{timeEnd}")]
        public async Task<IActionResult> GetTopicsOpenedBySupport(DateTime timeStart, DateTime timeEnd)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (timeStart > timeEnd)
                return ResponseUtil.Conflict("Начало промежутка времени больше конечного");

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .Where(st => st.Date >= timeStart && st.Date <= timeEnd)
                .ToListAsync();

            return ResponseUtil.Ok(topics);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("support")]
        public async Task<IActionResult> GetTopicsBySupport()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .ToListAsync();

            return ResponseUtil.Ok(topics);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("{id}/support")]
        public async Task<IActionResult> GetTopicBySupport(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id);

            return topic is null ?
                ResponseUtil.NotFound("Топик не найден") :
                ResponseUtil.Ok(topic.Convert(false));
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("support/answer/{id}")]
        public async Task<IActionResult> GetAnswerBySupport(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .Include(sta => sta.Plaintiff)
                .Include(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == id);

            return answer is null ?
                ResponseUtil.NotFound("Ответ на топик не найден") :
                ResponseUtil.Ok(answer);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("{id}/support/answers")]
        public async Task<IActionResult> GetAnswersBySupport(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            List<SupportTopicAnswer> answers = await context.SupportTopicAnswers
                .Include(sta => sta.Plaintiff)
                .Include(sta => sta.Images)
                .AsNoTracking()
                .Where(sta => sta.TopicId == id)
                .ToListAsync();

            return ResponseUtil.Ok(answers);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPost]
        public async Task<IActionResult> Create(SupportTopicDto topicDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            topicDto.UserId = UserId;
            topicDto.IsClosed = false;
            topicDto.Date = DateTime.UtcNow;

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .Where(st => st.UserId == topicDto.UserId && st.IsClosed == false)
                .ToListAsync();

            return topics.Count >= 3 ?
                ResponseUtil.Conflict("Количество открытых топиков не может превышать 3") : 
                await EndpointUtil.Create(topicDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPost("answer")]
        public async Task<IActionResult> CreateAnswer(SupportTopicAnswerDto answerDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .FirstOrDefaultAsync(st => st.Id == answerDto.TopicId && st.UserId == UserId);

            if (topic is null)
                return ResponseUtil.NotFound("Топик не найден");

            answerDto.PlaintiffId = UserId;
            answerDto.Date = DateTime.UtcNow;

            return await EndpointUtil.Create(answerDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPost("support/answer")]
        public async Task<IActionResult> CreateAnswerBySupport(SupportTopicAnswerDto answerDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == answerDto.TopicId);

            if (topic is null)
                return ResponseUtil.NotFound("Топик не найден");

            answerDto.PlaintiffId = UserId;
            answerDto.Date = DateTime.UtcNow;

            return await EndpointUtil.Create(answerDto.Convert(), context);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPut]
        public async Task<IActionResult> Update(SupportTopicDto topicDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .FirstOrDefaultAsync(st => st.Id == topicDto.Id && st.UserId == UserId);

            if (topic is null)
                return ResponseUtil.NotFound("Топик не найден");

            topicDto.UserId = topic.UserId;
            topicDto.Date = topic.Date;

            return await EndpointUtil.Update(topic, topicDto.Convert(false), context);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpGet("{id}/close")]
        public async Task<IActionResult> Close(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? topic = await context.SupportTopics
                .FirstOrDefaultAsync(st => st.Id == id);

            if (topic is null)
                return ResponseUtil.NotFound("Топик не найден");

            topic.IsClosed = true;
            topic.Date = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return ResponseUtil.Ok(topic.Convert(false));
        }

        [AuthorizeRoles(Roles.UserAdminOwner)]
        [HttpPut("answer")]
        public async Task<IActionResult> UpdateAnswer(SupportTopicAnswerDto answerDto)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .FirstOrDefaultAsync(sta => 
                sta.Id == answerDto.Id && 
                sta.PlaintiffId == UserId && 
                sta.TopicId == answerDto.TopicId);

            if (answer is null)
                return ResponseUtil.NotFound("Ответ на топик не найден");

            answerDto.PlaintiffId = UserId;
            answerDto.Date = DateTime.UtcNow;

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
                .FirstOrDefaultAsync(sta => sta.Id == id && sta.PlaintiffId == UserId);

            return answer is null ?
                ResponseUtil.NotFound("Ответ на топик не найден") : 
                await EndpointUtil.Delete(answer, context);
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
