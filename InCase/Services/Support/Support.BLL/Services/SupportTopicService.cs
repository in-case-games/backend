using Microsoft.EntityFrameworkCore;
using Support.BLL.Exceptions;
using Support.BLL.Helpers;
using Support.BLL.Interfaces;
using Support.BLL.Models;
using Support.DAL.Data;
using Support.DAL.Entities;

namespace Support.BLL.Services
{
    public class SupportTopicService : ISupportTopicService
    {
        private readonly ApplicationDbContext _context;

        public SupportTopicService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SupportTopicResponse> GetAsync(Guid userId, Guid id)
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
                topic.ToResponse() :
                throw new ForbiddenException("Вы не создатель топика");
        }

        public async Task<SupportTopicResponse> GetAsync(Guid id)
        {
            SupportTopic topic = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id) ??
                throw new NotFoundException("Топик не найден");

            return topic.ToResponse();
        }

        public async Task<List<SupportTopicResponse>> GetByUserIdAsync(Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            List<SupportTopic> topics = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .Where(st => st.UserId == userId)
                .ToListAsync();

            return topics.ToResponse();
        }

        public async Task<List<SupportTopicResponse>> GetOpenedTopicsAsync()
        {
            List<SupportTopic> topics = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .Where(st => st.IsClosed == false)
                .ToListAsync();

            return topics.ToResponse();
        }

        public async Task<SupportTopicResponse> CloseTopic(Guid id)
        {
            SupportTopic topic = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .FirstOrDefaultAsync(st => st.Id == id) ??
                throw new NotFoundException("Топик не найден");

            topic.IsClosed = true;

            await _context.SaveChangesAsync();

            return topic.ToResponse();
        }

        public async Task<SupportTopicResponse> CloseTopic(Guid userId, Guid id)
        {
            SupportTopic topic = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .FirstOrDefaultAsync(st => st.Id == id) ??
                throw new NotFoundException("Топик не найден");

            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");
            if (topic.UserId != userId)
                throw new ForbiddenException("Вы не создатель топика");

            topic.IsClosed = true;

            await _context.SaveChangesAsync();

            return topic.ToResponse();
        }

        public async Task<SupportTopicResponse> CreateAsync(SupportTopicRequest request)
        {
            List<SupportTopic> topics = await _context.Topics
                .AsNoTracking()
                .Where(st => st.UserId == request.UserId && st.IsClosed == false)
                .ToListAsync();
            SupportTopic topic = request.ToEntity(isNewGuid: true);

            if (!await _context.Users.AnyAsync(u => u.Id == request.UserId))
                throw new NotFoundException("Пользователь не найден");
            if (topics.Count >= 3)
                throw new ConflictException("Количество открытых топиков не может превышать 3");

            topic.IsClosed = false;
            topic.Date = DateTime.UtcNow;

            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();

            return topic.ToResponse();
        }

        public async Task<SupportTopicResponse> UpdateAsync(SupportTopicRequest request)
        {
            SupportTopic topicOld = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .FirstOrDefaultAsync(st => st.Id == request.Id) ??
                throw new NotFoundException("Топик не найден");
            SupportTopic topic = request.ToEntity(isNewGuid: false);

            if (!await _context.Users.AnyAsync(u => u.Id == request.UserId))
                throw new NotFoundException("Пользователь не найден");
            if (topicOld.UserId != request.UserId)
                throw new ForbiddenException("Вы не создатель топика");

            topic.Date = topicOld.Date;
            topic.IsClosed = topicOld.IsClosed;

            _context.Entry(topicOld).CurrentValues.SetValues(topic);
            await _context.SaveChangesAsync();

            topic.Answers = topicOld.Answers;

            return topic.ToResponse();
        }

        public async Task<SupportTopicResponse> DeleteAsync(Guid userId, Guid id)
        {
            SupportTopic topic = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id) ??
                throw new NotFoundException("Топик не найден");

            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");
            if (topic.UserId != userId)
                throw new ForbiddenException("Вы не создатель топика");

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return topic.ToResponse();
        }

        public async Task<SupportTopicResponse> DeleteAsync(Guid id)
        {
            SupportTopic topic = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id) ??
                throw new NotFoundException("Топик не найден");

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return topic.ToResponse();
        }
    }
}
