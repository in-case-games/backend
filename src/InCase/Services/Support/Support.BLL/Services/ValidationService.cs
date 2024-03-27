using Support.BLL.Exceptions;
using Support.BLL.Models;

namespace Support.BLL.Services;
public static class ValidationService
{
    public static void IsSupportTopicAnswer(SupportTopicAnswerRequest request)
    {
        if (request.Content is null || request.Content.Length > 100 || request.Content.Length < 5)
            throw new BadRequestException("Длина сообщения должна находится между 5 и 100 символами");
    }

    public static void IsSupportTopic(SupportTopicRequest request)
    {
        if (request.Content is null || request.Content.Length > 25 || request.Content.Length < 5)
            throw new BadRequestException("Длина заголовка должна находится между 5 и 25 символами");
        if (request.Content is null || request.Content.Length > 100 || request.Content.Length < 5)
            throw new BadRequestException("Длина сообщения должна находится между 5 и 100 символами");
    }
}