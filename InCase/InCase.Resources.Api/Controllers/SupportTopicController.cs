using InCase.Domain.Common;
using InCase.Domain.Dtos;
using InCase.Domain.Entities.Resources;
using InCase.Infrastructure.Data;
using InCase.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InCase.Resources.Api.Controllers
{
    [Route("api/support-topic")]
    [ApiController]
    public class SupportTopicController : ControllerBase
    {

        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public SupportTopicController(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IEnumerable<SupportTopic> supportTopics = await context.SupportTopics
                .Include(x => x.Answers)
                .Include(x => x.User)
                .ToListAsync();

            return ResponseUtil.Ok(supportTopics);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? supportTopic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supportTopic is null)
                return ResponseUtil.NotFound(nameof(SupportTopic));

            return ResponseUtil.Ok(supportTopic);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("{id}/answers")]
        public async Task<IActionResult> GetAnswers(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            IEnumerable<SupportTopicAnswer> answers = await context.SupportTopicAnswers
                .Include(x => x.Plaintiff)
                .Where(x => x.TopicId == id)
                .Include(x => x.Images)
                .ToListAsync();

            return ResponseUtil.Ok(answers);
        }
        [AuthorizeRoles(Roles.All)]
        [HttpGet("{id}/answers/{answerId}")]
        public async Task<IActionResult> GetAnswer(Guid id, Guid answerId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .Include(x => x.Plaintiff)
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.TopicId == id && x.Id == answerId);

            if (answer is null)
                return ResponseUtil.NotFound(nameof(SupportTopicAnswer));

            return ResponseUtil.Ok(answer);
        }
        [AuthorizeRoles(Roles.Admin, Roles.User, Roles.Support)]
        [HttpPost]
        public async Task<IActionResult> Create(SupportTopicDto supportTopic)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                await context.SupportTopics.AddAsync(supportTopic.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(supportTopic);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin, Roles.User, Roles.Support)]
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
                    return ResponseUtil.NotFound(nameof(SupportTopic));
                }

                supportTopicAnswer.TopicId = id;

                await context.SupportTopicAnswers.AddAsync(supportTopicAnswer.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(supportTopicAnswer);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin, Roles.Support)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, SupportTopicDto supportTopic)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            try
            {
                supportTopic.Id = id;
                context.SupportTopics.Update(supportTopic.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(supportTopic);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin, Roles.Support)]
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
                    return ResponseUtil.NotFound(nameof(SupportTopicAnswer));
                }

                answer.Id = id;
                context.SupportTopicAnswers.Update(answer.Convert());
                await context.SaveChangesAsync();

                return ResponseUtil.Ok(answer);
            }
            catch (Exception ex)
            {
                return ResponseUtil.Error(ex);
            }
        }
        [AuthorizeRoles(Roles.Admin, Roles.Support)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopic? supportTopic = await context.SupportTopics
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supportTopic is null)
                return ResponseUtil.NotFound(nameof(SupportTopic));

            context.SupportTopics.Remove(supportTopic);
            await context.SaveChangesAsync();

            return ResponseUtil.Delete(nameof(SupportTopic));
        }
        [AuthorizeRoles(Roles.Admin, Roles.Support)]
        [HttpDelete("{id}/answers/{answerId}")]
        public async Task<IActionResult> DeleteAnswer(Guid id, Guid answerId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .FirstOrDefaultAsync(x => x.TopicId == id && x.Id == answerId);

            if (answer is null)
                return ResponseUtil.NotFound(nameof(SupportTopicAnswer));

            context.SupportTopicAnswers.Remove(answer);
            await context.SaveChangesAsync();

            return ResponseUtil.Delete(nameof(SupportTopicAnswer));
        }
    }
}
