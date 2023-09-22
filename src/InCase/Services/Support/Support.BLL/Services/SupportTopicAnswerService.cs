using Microsoft.EntityFrameworkCore;
using Support.BLL.Exceptions;
using Support.BLL.Helpers;
using Support.BLL.Interfaces;
using Support.BLL.Models;
using Support.DAL.Data;
using Support.DAL.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace Support.BLL.Services
{
    public class SupportTopicAnswerService : ISupportTopicAnswerService
    {
        private readonly ApplicationDbContext _context;

        public SupportTopicAnswerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SupportTopicAnswerResponse> GetAsync(Guid userId, Guid id)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            SupportTopicAnswer answer = await _context.Answers
                .Include(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == id) ??
                throw new NotFoundException("Сообщение не найдено");

            SupportTopic topic = await _context.Topics
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == answer.TopicId) ??
                throw new NotFoundException("Топик не найден");

            return topic.UserId == userId ? 
                answer.ToResponse() :
                throw new ForbiddenException("Вы не создатель топика");
        }

        public async Task<SupportTopicAnswerResponse> GetAsync(Guid id)
        {
            SupportTopicAnswer answer = await _context.Answers
                .Include(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == id) ??
                throw new NotFoundException("Сообщение не найдено");

            return answer.ToResponse();
        }

        public async Task<List<SupportTopicAnswerResponse>> GetByUserIdAsync(Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            List<SupportTopic> topics = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .Where(st => st.UserId == userId)
                .ToListAsync();

            List<SupportTopicAnswerResponse> response = new();

            foreach (var topic in topics)
                response.AddRange(topic.Answers!.ToResponse());

            return response;
        }

        public async Task<List<SupportTopicAnswerResponse>> GetByTopicIdAsync(Guid userId, Guid id)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            SupportTopic topic = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id) ??
                throw new NotFoundException("Топик не найден");

            return topic.UserId == userId ? 
                topic.Answers!.ToResponse() : 
                throw new ForbiddenException("Вы не создатель топика");
        }

        public async Task<List<SupportTopicAnswerResponse>> GetByTopicIdAsync(Guid id)
        {
            SupportTopic topic = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id) ??
                throw new NotFoundException("Топик не найден");

            return topic.Answers!.ToResponse();
        }

        public async Task<SupportTopicAnswerResponse> CreateAsync(SupportTopicAnswerRequest request)
        {
            SupportTopic topic = await _context.Topics
                .FirstOrDefaultAsync(st => st.Id == request.TopicId) ??
                throw new NotFoundException("Топик не найден");
            SupportTopicAnswer answer = request.ToEntity(isNewGuid: true);

            answer.Date = DateTime.UtcNow;
            topic.IsClosed = false;

            if (!await _context.Users.AnyAsync(u => u.Id == request.PlaintiffId))
                throw new NotFoundException("Пользователь не найден");
            if (topic.UserId != request.PlaintiffId)
                throw new ForbiddenException("Вы не создатель топика");

            FileService.CreateFolder(@$"topic-answers\{answer.TopicId}\{request.Id}\");

            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();

            return answer.ToResponse();
        }

        public async Task<SupportTopicAnswerResponse> CreateByAdminAsync(SupportTopicAnswerRequest request)
        {
            if (!await _context.Topics.AnyAsync(st => st.Id == request.TopicId))
                throw new NotFoundException("Топик не найден");

            request.Date = DateTime.UtcNow;

            SupportTopicAnswer answer = request.ToEntity(isNewGuid: true);

            FileService.CreateFolder(@$"topic-answers\{answer.TopicId}\{request.Id}\");

            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();

            return answer.ToResponse();
        }

        public async Task<SupportTopicAnswerResponse> UpdateAsync(SupportTopicAnswerRequest request)
        {
            SupportTopicAnswer answerOld = await _context.Answers
                .FirstOrDefaultAsync(sta => sta.Id == request.Id) ??
                throw new NotFoundException("Ответ не найден");
            SupportTopicAnswer answer = request.ToEntity();

            answer.Date = DateTime.UtcNow;

            if (answerOld.PlaintiffId != request.PlaintiffId)
                throw new ForbiddenException("Вы не создатель сообщения");
            if (!await _context.Users.AnyAsync(u => u.Id == request.PlaintiffId))
                throw new NotFoundException("Пользователь не найден");
            if (!await _context.Topics.AnyAsync(st => st.Id == request.TopicId))
                throw new NotFoundException("Топик не найден");

            _context.Entry(answerOld).CurrentValues.SetValues(answer);
            await _context.SaveChangesAsync();

            return answer.ToResponse();
        }

        public async Task<SupportTopicAnswerResponse> DeleteAsync(Guid userId, Guid id)
        {
            SupportTopicAnswer answer = await _context.Answers
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == id) ??
                throw new NotFoundException("Ответ не найден");

            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");
            if (answer.PlaintiffId != userId)
                throw new ForbiddenException("Вы не создатель сообщения");

            FileService.RemoveFolder(@$"topic-answers\{answer.TopicId}\{id}\");

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();

            return answer.ToResponse();
        }
    }
}
