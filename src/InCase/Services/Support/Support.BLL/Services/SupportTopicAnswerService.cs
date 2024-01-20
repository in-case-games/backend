using Microsoft.EntityFrameworkCore;
using Support.BLL.Exceptions;
using Support.BLL.Helpers;
using Support.BLL.Interfaces;
using Support.BLL.Models;
using Support.DAL.Data;

namespace Support.BLL.Services
{
    public class SupportTopicAnswerService(ApplicationDbContext context) : ISupportTopicAnswerService
    {
        public async Task<SupportTopicAnswerResponse> GetAsync(Guid userId, Guid id, CancellationToken cancellation = default)
        {
            if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
                throw new NotFoundException("Пользователь не найден");

            var answer = await context.Answers
                .Include(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == id, cancellation) ??
                throw new NotFoundException("Сообщение не найдено");
            var topic = await context.Topics
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == answer.TopicId, cancellation) ??
                throw new NotFoundException("Топик не найден");

            return topic.UserId == userId ? 
                answer.ToResponse() :
                throw new ForbiddenException("Вы не создатель топика");
        }

        public async Task<SupportTopicAnswerResponse> GetAsync(Guid id, CancellationToken cancellation = default)
        {
            var answer = await context.Answers
                .Include(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == id, cancellation) ??
                throw new NotFoundException("Сообщение не найдено");

            return answer.ToResponse();
        }

        public async Task<List<SupportTopicAnswerResponse>> GetByUserIdAsync(Guid userId, 
            CancellationToken cancellation = default)
        {
            if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
                throw new NotFoundException("Пользователь не найден");

            var topics = await context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .Where(st => st.UserId == userId)
                .ToListAsync(cancellation);

            var response = new List<SupportTopicAnswerResponse>();

            foreach (var topic in topics) response.AddRange(topic.Answers!.ToResponse());

            return response;
        }

        public async Task<List<SupportTopicAnswerResponse>> GetByTopicIdAsync(Guid userId, Guid id, CancellationToken cancellation = default)
        {
            if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
                throw new NotFoundException("Пользователь не найден");

            var topic = await context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id, cancellation) ??
                throw new NotFoundException("Топик не найден");

            return topic.UserId == userId ? 
                topic.Answers!.ToResponse() : 
                throw new ForbiddenException("Вы не создатель топика");
        }

        public async Task<List<SupportTopicAnswerResponse>> GetByTopicIdAsync(Guid id, CancellationToken cancellation = default)
        {
            var topic = await context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id, cancellation) ??
                throw new NotFoundException("Топик не найден");

            return topic.Answers!.ToResponse();
        }

        public async Task<SupportTopicAnswerResponse> CreateAsync(SupportTopicAnswerRequest request, CancellationToken cancellation = default)
        {
            ValidationService.IsSupportTopicAnswer(request);

            var topic = await context.Topics
                .FirstOrDefaultAsync(st => st.Id == request.TopicId, cancellation) ??
                throw new NotFoundException("Топик не найден");
            var answer = request.ToEntity(isNewGuid: true);

            answer.Date = DateTime.UtcNow;
            topic.IsClosed = false;

            if (topic.UserId != request.PlaintiffId) 
                throw new ForbiddenException("Вы не создатель топика");
            if (!await context.Users.AnyAsync(u => u.Id == request.PlaintiffId, cancellation))
                throw new NotFoundException("Пользователь не найден");

            await context.Answers.AddAsync(answer, cancellation);
            await context.SaveChangesAsync(cancellation);

            FileService.CreateFolder(@$"topic-answers/{answer.TopicId}/{request.Id}/");

            return answer.ToResponse();
        }

        public async Task<SupportTopicAnswerResponse> CreateByAdminAsync(SupportTopicAnswerRequest request, CancellationToken cancellation = default)
        {
            ValidationService.IsSupportTopicAnswer(request);

            if (!await context.Topics.AnyAsync(st => st.Id == request.TopicId, cancellation))
                throw new NotFoundException("Топик не найден");

            request.Date = DateTime.UtcNow;

            var answer = request.ToEntity(isNewGuid: true);

            await context.Answers.AddAsync(answer, cancellation);
            await context.SaveChangesAsync(cancellation);

            FileService.CreateFolder(@$"topic-answers/{answer.TopicId}/{request.Id}/");

            return answer.ToResponse();
        }

        public async Task<SupportTopicAnswerResponse> UpdateAsync(SupportTopicAnswerRequest request, CancellationToken cancellation = default)
        {
            ValidationService.IsSupportTopicAnswer(request);

            var answerOld = await context.Answers
                .FirstOrDefaultAsync(sta => sta.Id == request.Id, cancellation) ??
                throw new NotFoundException("Ответ не найден");
            var answer = request.ToEntity();

            answer.Date = DateTime.UtcNow;

            if (answerOld.PlaintiffId != request.PlaintiffId)
                throw new ForbiddenException("Вы не создатель сообщения");
            if (!await context.Users.AnyAsync(u => u.Id == request.PlaintiffId, cancellation))
                throw new NotFoundException("Пользователь не найден");
            if (!await context.Topics.AnyAsync(st => st.Id == request.TopicId, cancellation))
                throw new NotFoundException("Топик не найден");

            context.Entry(answerOld).CurrentValues.SetValues(answer);
            await context.SaveChangesAsync(cancellation);

            return answer.ToResponse();
        }

        public async Task<SupportTopicAnswerResponse> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default)
        {
            var answer = await context.Answers
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == id, cancellation) ??
                throw new NotFoundException("Ответ не найден");

            if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
                throw new NotFoundException("Пользователь не найден");
            if (answer.PlaintiffId != userId)
                throw new ForbiddenException("Вы не создатель сообщения");

            context.Answers.Remove(answer);
            await context.SaveChangesAsync(cancellation);

            FileService.RemoveFolder($"topic-answers/{answer.TopicId}/{id}/");

            return answer.ToResponse();
        }
    }
}
