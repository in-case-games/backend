using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Support.BLL.Exceptions;
using Support.BLL.Helpers;
using Support.BLL.Interfaces;
using Support.BLL.Models;
using Support.DAL.Data;
using Support.DAL.Entities;

namespace Support.BLL.Services
{
    public class AnswerImageService : IAnswerImageService
    {
        private readonly ApplicationDbContext _context;

        public AnswerImageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AnswerImageResponse> GetAsync(Guid userId, Guid id)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            AnswerImage image = await _context.Images
                .AsNoTracking()
                .FirstOrDefaultAsync(ai => ai.Id == id) ??
                throw new NotFoundException("Картинка не найдена");

            SupportTopicAnswer answer = await _context.Answers
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == image.AnswerId) ??
                throw new NotFoundException("Сообщение не найдено");

            SupportTopic topic = await _context.Topics
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == answer.TopicId) ??
                throw new NotFoundException("Топик не найден");

            return topic.UserId == userId ? 
                image.ToResponse() : 
                throw new ForbiddenException("Вы не создатель сообщения"); 
        }

        public async Task<AnswerImageResponse> GetAsync(Guid id)
        {
            AnswerImage image = await _context.Images
                .AsNoTracking()
                .FirstOrDefaultAsync(ai => ai.Id == id) ??
                throw new NotFoundException("Картинка не найдена");

            return image.ToResponse();
        }

        public async Task<List<AnswerImageResponse>> GetByAnswerIdAsync(Guid userId, Guid id)
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
                answer.Images!.ToResponse() : 
                throw new ForbiddenException("Вы не создатель сообщения");
        }

        public async Task<List<AnswerImageResponse>> GetByAnswerIdAsync(Guid id)
        {
            SupportTopicAnswer answer = await _context.Answers
                .Include(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == id) ??
                throw new NotFoundException("Сообщение не найдено");

            return answer.Images!.ToResponse();
        }

        public async Task<List<AnswerImageResponse>> GetByTopicIdAsync(Guid userId, Guid id)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            SupportTopic topic = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id) ??
                throw new NotFoundException("Топик не найден");

            if (topic.UserId != userId)
                throw new ForbiddenException("Вы не создатель сообщения");

            List<AnswerImageResponse> response = new();

            foreach (var answer in topic.Answers!)
                response.AddRange(answer.Images!.ToResponse());

            return response;
        }

        public async Task<List<AnswerImageResponse>> GetByUserIdAsync(Guid userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            List<SupportTopicAnswer> answers = await _context.Answers
                .Include(sta => sta.Images)
                .AsNoTracking()
                .Where(st => st.PlaintiffId == userId)
                .ToListAsync();

            List<AnswerImageResponse> response = new();
            
            foreach (var answer in answers)
                response.AddRange(answer.Images!.ToResponse());

            return response;
        }

        public async Task<List<AnswerImageResponse>> GetByTopicIdAsync(Guid id)
        {
            SupportTopic topic = await _context.Topics
                .Include(st => st.Answers!)
                    .ThenInclude(sta => sta.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == id) ??
                throw new NotFoundException("Топик не найден");

            List<AnswerImageResponse> response = new();

            foreach (var answer in topic.Answers!)
                response.AddRange(answer.Images!.ToResponse());

            return response;
        }

        public async Task<AnswerImageResponse> CreateAsync(Guid userId, 
            AnswerImageRequest request,
            IFormFile uploadImage)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            SupportTopicAnswer answer = await _context.Answers
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == request.AnswerId) ??
                throw new NotFoundException("Сообщение не найдено");

            if (answer.PlaintiffId != userId)
                throw new ForbiddenException("Вы не создатель сообщения");

            AnswerImage image = request.ToEntity(isNewGuid: true);

            string[] currentDirPath = Environment.CurrentDirectory.Split("src");
            string path = currentDirPath[0];

            FileService.Upload(uploadImage, 
               "answers\\{image.AnswerId}\\{image.Id}\\" + image.Id + ".jpg");

            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();

            return image.ToResponse();
        }

        public async Task<AnswerImageResponse> DeleteAsync(Guid userId, Guid id)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new NotFoundException("Пользователь не найден");

            AnswerImage image = await _context.Images
                .AsNoTracking()
                .FirstOrDefaultAsync(ai => ai.Id == id) ??
                throw new NotFoundException("Картинка не найдена");

            SupportTopicAnswer answer = await _context.Answers
                .AsNoTracking()
                .FirstOrDefaultAsync(sta => sta.Id == image.AnswerId) ??
                throw new NotFoundException("Сообщение не найдено");

            SupportTopic topic = await _context.Topics
                .AsNoTracking()
                .FirstOrDefaultAsync(st => st.Id == answer.TopicId) ??
                throw new NotFoundException("Топик не найден");

            if (answer.PlaintiffId != userId)
                throw new ForbiddenException("Вы не создатель сообщения");

            string[] currentDirPath = Environment.CurrentDirectory.Split("src");
            string path = currentDirPath[0];

            File.Delete("answers\\{image.AnswerId}\\{image.Id}\\" + image.Id + ".jpg");
            FileService.RemoveFolder("answers\\{image.AnswerId}\\{image.Id}");

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return image.ToResponse();
        }

        public async Task<AnswerImageResponse> DeleteAsync(Guid id)
        {
            AnswerImage image = await _context.Images
                .AsNoTracking()
                .FirstOrDefaultAsync(ai => ai.Id == id) ??
                throw new NotFoundException("Картинка не найдена");

            string[] currentDirPath = Environment.CurrentDirectory.Split("src");
            string path = currentDirPath[0];

            File.Delete("answers\\{image.AnswerId}\\{image.Id}\\" + image.Id + ".jpg");
            FileService.RemoveFolder("answers\\{image.AnswerId}\\{image.Id}");

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return image.ToResponse();
        }
    }
}
