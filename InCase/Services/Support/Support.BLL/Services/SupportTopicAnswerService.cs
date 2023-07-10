using Microsoft.EntityFrameworkCore;
using Support.BLL.Exceptions;
using Support.BLL.Helpers;
using Support.BLL.Interfaces;
using Support.BLL.Models;
using Support.DAL.Data;
using Support.DAL.Entities;
using System.Threading;

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
            SupportTopicAnswer answer = await _context.Answers
                .Include(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == id) ??
                throw new NotFoundException("Сообщение не найдено");

            if (!await _context.Topics.AnyAsync(st => st.UserId == userId && st.Id == answer.TopicId))
                throw new ForbiddenException("Только создатель топика может видеть сообщения");

            return answer.ToResponse();
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
            SupportTopic topic = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.UserId == userId && st.Id == id) ??
                throw new ForbiddenException("Только создатель топика может видеть сообщения");

            return topic.Answers!.ToResponse();
        }

        public async Task<List<SupportTopicAnswerResponse>> GetByTopicIdAsync(Guid id)
        {
            SupportTopic topic = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id) ??
                throw new ForbiddenException("Топик не найден");

            return topic.Answers!.ToResponse();
        }

        public async Task<SupportTopicAnswerResponse> CreateAsync(SupportTopicAnswerRequest request)
        {
            if (!await _context.Topics.AnyAsync(st => st.UserId == request.PlaintiffId && st.Id == request.TopicId))
                throw new ForbiddenException("Топик не найден");

            request.Date = DateTime.UtcNow;

            SupportTopicAnswer answer = request.ToEntity(isNewGuid: true);

            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();

            return answer.ToResponse();
        }

        public async Task<SupportTopicAnswerResponse> CreateByAdminAsync(SupportTopicAnswerRequest request)
        {
            if (!await _context.Topics.AnyAsync(st => st.Id == request.TopicId))
                throw new ForbiddenException("Топик не найден");

            request.Date = DateTime.UtcNow;

            SupportTopicAnswer answer = request.ToEntity(isNewGuid: true);

            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();

            return answer.ToResponse();
        }

        public Task<SupportTopicAnswerResponse> UpdateAsync(SupportTopicAnswerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> UpdateByAdminAsync(SupportTopicAnswerRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<SupportTopicAnswerResponse> DeleteAsync(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
