using Microsoft.EntityFrameworkCore;
using Support.BLL.Exceptions;
using Support.BLL.Helpers;
using Support.BLL.Interfaces;
using Support.BLL.Models;
using Support.DAL.Data;
using Support.DAL.Entities;

namespace Support.BLL.Services;

public class AnswerImageService(ApplicationDbContext context) : IAnswerImageService
{
    public async Task<AnswerImageResponse> GetAsync(Guid userId, Guid id, CancellationToken cancellation = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var image = await context.Images
            .AsNoTracking()
            .FirstOrDefaultAsync(ai => ai.Id == id, cancellation) ??
            throw new NotFoundException("Картинка не найдена");
        var answer = await context.Answers
            .AsNoTracking()
            .FirstOrDefaultAsync(sta => sta.Id == image.AnswerId, cancellation) ??
            throw new NotFoundException("Сообщение не найдено");
        var topic = await context.Topics
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.Id == answer.TopicId, cancellation) ??
            throw new NotFoundException("Топик не найден");

        return topic.UserId == userId ? 
            image.ToResponse() : 
            throw new ForbiddenException("Вы не создатель сообщения"); 
    }

    public async Task<AnswerImageResponse> GetAsync(Guid id, CancellationToken cancellation = default)
    {
        var image = await context.Images
            .AsNoTracking()
            .FirstOrDefaultAsync(ai => ai.Id == id, cancellation) ??
            throw new NotFoundException("Картинка не найдена");

        return image.ToResponse();
    }

    public async Task<List<AnswerImageResponse>> GetByAnswerIdAsync(Guid userId, Guid id, 
        CancellationToken cancellation = default)
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
            answer.Images!.ToResponse() : 
            throw new ForbiddenException("Вы не создатель сообщения");
    }

    public async Task<List<AnswerImageResponse>> GetByAnswerIdAsync(Guid id, CancellationToken cancellation = default)
    {
        var answer = await context.Answers
            .Include(sta => sta.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(sta => sta.Id == id, cancellation) ??
            throw new NotFoundException("Сообщение не найдено");

        return answer.Images!.ToResponse();
    }

    public async Task<List<AnswerImageResponse>> GetByTopicIdAsync(Guid userId, Guid id, 
        CancellationToken cancellation = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var topic = await context.Topics
            .Include(st => st.Answers!)
                .ThenInclude(sta => sta.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.Id == id, cancellation) ??
            throw new NotFoundException("Топик не найден");

        if (topic.UserId != userId) throw new ForbiddenException("Вы не создатель сообщения");

        return topic.Answers!.ToAnswerImageResponse();
    }

    public async Task<List<AnswerImageResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellation = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var answers = await context.Answers
            .Include(sta => sta.Images)
            .AsNoTracking()
            .Where(st => st.PlaintiffId == userId)
            .ToListAsync(cancellation);

        return answers.ToAnswerImageResponse();
    }

    public async Task<List<AnswerImageResponse>> GetByTopicIdAsync(Guid id, CancellationToken cancellation = default)
    {
        var topic = await context.Topics
            .Include(st => st.Answers!)
                .ThenInclude(sta => sta.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.Id == id, cancellation) ??
            throw new NotFoundException("Топик не найден");

        return topic.Answers!.ToAnswerImageResponse();
    }

    public async Task<AnswerImageResponse> CreateAsync(Guid userId, AnswerImageRequest request, 
        CancellationToken cancellation = default)
    {
        if (request.Image is null) throw new BadRequestException("Загрузите картинку в base 64");

        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var answer = await context.Answers
            .AsNoTracking()
            .FirstOrDefaultAsync(sta => sta.Id == request.AnswerId, cancellation) ??
            throw new NotFoundException("Сообщение не найдено");

        if (answer.PlaintiffId != userId) throw new ForbiddenException("Вы не создатель сообщения");

        var image = new AnswerImage
        {
            Id = Guid.NewGuid(),
            AnswerId = request.AnswerId,
        };

        await context.Images.AddAsync(image, cancellation);
        await context.SaveChangesAsync(cancellation);

        FileService.UploadImageBase64(request.Image, 
            $"topic-answers/{answer.TopicId}/{image.AnswerId}/{image.Id}/", 
            $"{image.Id}");

        return image.ToResponse();
    }

    public async Task<AnswerImageResponse> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default)
    {
        if (!await context.Users.AnyAsync(u => u.Id == userId, cancellation))
            throw new NotFoundException("Пользователь не найден");

        var image = await context.Images
            .AsNoTracking()
            .FirstOrDefaultAsync(ai => ai.Id == id, cancellation) ??
            throw new NotFoundException("Картинка не найдена");
        var answer = await context.Answers
            .AsNoTracking()
            .FirstOrDefaultAsync(sta => sta.Id == image.AnswerId, cancellation) ??
            throw new NotFoundException("Сообщение не найдено");
        if(!await context.Topics.AnyAsync(st => st.Id == answer.TopicId, cancellation))
            throw new NotFoundException("Топик не найден");

        if (answer.PlaintiffId != userId) throw new ForbiddenException("Вы не создатель сообщения");

        context.Images.Remove(image);
        await context.SaveChangesAsync(cancellation);

        FileService.RemoveFolder($"topic-answers/{answer.TopicId}/{image.AnswerId}/{id}/");

        return image.ToResponse();
    }

    public async Task<AnswerImageResponse> DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var image = await context.Images
            .AsNoTracking()
            .FirstOrDefaultAsync(ai => ai.Id == id, cancellation) ??
            throw new NotFoundException("Картинка не найдена");
        var answer = await context.Answers
            .AsNoTracking()
            .FirstOrDefaultAsync(sta => sta.Id == image.AnswerId, cancellation) ??
            throw new NotFoundException("Сообщение не найдено");

        context.Images.Remove(image);
        await context.SaveChangesAsync(cancellation);

        FileService.RemoveFolder($"topic-answers/{answer.TopicId}/{image.AnswerId}/{id}/");

        return image.ToResponse();
    }
}