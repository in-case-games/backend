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
            return await EndpointUtil.GetById<SupportTopic>(id, _contextFactory);
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
            return await EndpointUtil.Create(supportTopic.Convert(), _contextFactory);
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
        [HttpPut()]
        public async Task<IActionResult> Update(SupportTopicDto supportTopic)
        {
            return await EndpointUtil.Update(supportTopic.Convert(), _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Support)]
        [HttpPut("answers")]
        public async Task<IActionResult> UpdateAnswer(SupportTopicAnswerDto answer)
        {
            return await EndpointUtil.Update(answer.Convert(), _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Support)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await EndpointUtil.Delete<SupportTopic>(id, _contextFactory);
        }
        [AuthorizeRoles(Roles.Admin, Roles.Support)]
        [HttpDelete("{id}/answers/{answerId}")]
        public async Task<IActionResult> DeleteAnswer(Guid id, Guid answerId)
        {
            await using ApplicationDbContext context = await _contextFactory.CreateDbContextAsync();

            if (await context.Set<SupportTopic>()
                .FindAsync(id) is null)
                return ResponseUtil.NotFound(nameof(SupportTopic));

            SupportTopicAnswer? answer = await context.SupportTopicAnswers
                .FirstOrDefaultAsync(x => x.TopicId == id && x.Id == answerId);

            if (answer is null)
                return ResponseUtil.NotFound(nameof(SupportTopicAnswer));

            context.Set<SupportTopicAnswer>().Remove(answer);
            await context.SaveChangesAsync();

            return ResponseUtil.Delete(nameof(SupportTopicAnswer));
        }
    }
}
