using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.CustomException;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Services;
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
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .Where(st => st.UserId == UserId)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(topics);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            SupportTopic topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id && st.UserId == UserId, cancellationToken) ??
                throw new NotFoundCodeException("Топик не найден");

            return ResponseUtil.Ok(topic);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}/answers")]
        public async Task<IActionResult> GetAnswers(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            await ValidationService.CheckOwnerSupportTopic(id, UserId, context);

            List<SupportTopicAnswer> answers = await context.SupportTopicAnswers
                .Include(sta => sta.Plaintiff)
                .Include(sta => sta.Images)
                .AsNoTracking()
                .Where(sta => sta.TopicId == id)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(answers);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpGet("{id}/answer/{answerId}")]
        public async Task<IActionResult> GetAnswer(Guid id, Guid answerId, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            await ValidationService.CheckOwnerSupportTopic(id, UserId, context);

            SupportTopicAnswer answer = await context.SupportTopicAnswers
                .Include(sta => sta.Plaintiff)
                .Include(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == answerId, cancellationToken) ??
                throw new NotFoundCodeException("Ответ на топик не найден");

            return ResponseUtil.Ok(answer);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("support/opened")]
        public async Task<IActionResult> GetTopicsOpenedBySupport(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .Where(st => st.IsClosed == false)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(topics);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("support/{timeStart}&{timeEnd}")]
        public async Task<IActionResult> GetTopicsOpenedBySupport(DateTime timeStart, DateTime timeEnd, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            if (timeStart > timeEnd)
                throw new ConflictCodeException("Начало промежутка времени больше конечного");

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .Where(st => st.Date >= timeStart && st.Date <= timeEnd)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(topics);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("support")]
        public async Task<IActionResult> GetTopicsBySupport(CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(topics);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("{id}/support")]
        public async Task<IActionResult> GetTopicBySupport(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            SupportTopic topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id, cancellationToken) ??
                throw new NotFoundCodeException("Топик не найден");

            return ResponseUtil.Ok(topic.Convert(false));
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("support/answer/{id}")]
        public async Task<IActionResult> GetAnswerBySupport(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            SupportTopicAnswer answer = await context.SupportTopicAnswers
                .Include(sta => sta.Plaintiff)
                .Include(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == id, cancellationToken) ?? 
                throw new NotFoundCodeException("Ответ на топик не найден");

            return ResponseUtil.Ok(answer);
        }

        [AuthorizeRoles(Roles.AdminOwnerBot)]
        [HttpGet("{id}/support/answers")]
        public async Task<IActionResult> GetAnswersBySupport(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            List<SupportTopicAnswer> answers = await context.SupportTopicAnswers
                .Include(sta => sta.Plaintiff)
                .Include(sta => sta.Images)
                .AsNoTracking()
                .Where(sta => sta.TopicId == id)
                .ToListAsync(cancellationToken);

            return ResponseUtil.Ok(answers);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPost]
        public async Task<IActionResult> Create(SupportTopicDto topicDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            topicDto.UserId = UserId;
            topicDto.IsClosed = false;
            topicDto.Date = DateTime.UtcNow;

            List<SupportTopic> topics = await context.SupportTopics
                .AsNoTracking()
                .Where(st => st.UserId == topicDto.UserId && st.IsClosed == false)
                .ToListAsync(cancellationToken);

            return topics.Count >= 3 ?
                throw new ConflictCodeException("Количество открытых топиков не может превышать 3") : 
                await EndpointUtil.Create(topicDto.Convert(), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPost("answer")]
        public async Task<IActionResult> CreateAnswer(SupportTopicAnswerDto answerDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            SupportTopic topic = await context.SupportTopics
                .FirstOrDefaultAsync(st => st.Id == answerDto.TopicId && st.UserId == UserId, cancellationToken) ??
                throw new NotFoundCodeException("Топик не найден");

            answerDto.PlaintiffId = UserId;
            answerDto.Date = DateTime.UtcNow;

            return await EndpointUtil.Create(answerDto.Convert(), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpPost("support/answer")]
        public async Task<IActionResult> CreateAnswerBySupport(SupportTopicAnswerDto answerDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            SupportTopic topic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == answerDto.TopicId, cancellationToken) ??
                throw new NotFoundCodeException("Топик не найден");

            answerDto.PlaintiffId = UserId;
            answerDto.Date = DateTime.UtcNow;

            return await EndpointUtil.Create(answerDto.Convert(), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpPut]
        public async Task<IActionResult> Update(SupportTopicDto topicDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            SupportTopic topic = await context.SupportTopics
                .FirstOrDefaultAsync(st => st.Id == topicDto.Id && st.UserId == UserId, cancellationToken) ??
                throw new NotFoundCodeException("Топик не найден");

            topicDto.UserId = topic.UserId;
            topicDto.Date = topic.Date;

            return await EndpointUtil.Update(topic, topicDto.Convert(false), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpGet("{id}/close")]
        public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            SupportTopic topic = await context.SupportTopics
                .FirstOrDefaultAsync(st => st.Id == id, cancellationToken) ?? 
                throw new NotFoundCodeException("Топик не найден");

            topic.IsClosed = true;
            topic.Date = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            return ResponseUtil.Ok(topic.Convert(false));
        }

        [AuthorizeRoles(Roles.UserAdminOwner)]
        [HttpPut("answer")]
        public async Task<IActionResult> UpdateAnswer(SupportTopicAnswerDto answerDto, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            SupportTopicAnswer answer = await context.SupportTopicAnswers
                .FirstOrDefaultAsync(sta => 
                sta.Id == answerDto.Id && 
                sta.PlaintiffId == UserId && 
                sta.TopicId == answerDto.TopicId, cancellationToken) ??
                throw new NotFoundCodeException("Ответ на топик не найден");

            answerDto.PlaintiffId = UserId;
            answerDto.Date = DateTime.UtcNow;

            return await EndpointUtil.Update(answer, answerDto.Convert(false), context, cancellationToken);
        }

        [AuthorizeRoles(Roles.Owner)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            return await EndpointUtil.Delete<SupportTopic>(id, context, cancellationToken);
        }

        [AuthorizeRoles(Roles.User)]
        [HttpDelete("answer/{id}")]
        public async Task<IActionResult> DeleteAnswer(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            SupportTopicAnswer answer = await context.SupportTopicAnswers
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == id && sta.PlaintiffId == UserId, cancellationToken) ??
                throw new NotFoundCodeException("Ответ на топик не найден");

            return await EndpointUtil.Delete(answer, context, cancellationToken);
        }

        [AuthorizeRoles(Roles.Admin, Roles.Owner)]
        [HttpDelete("support/answer/{id}")]
        public async Task<IActionResult> DeleteAnswerBySupport(Guid id, CancellationToken cancellationToken)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            return await EndpointUtil.Delete<SupportTopicAnswer>(id, context, cancellationToken);
        }
    }
}
