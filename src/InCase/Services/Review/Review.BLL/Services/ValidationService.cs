using Review.BLL.Exceptions;
using Review.BLL.Models;

namespace Review.BLL.Services
{
    public static class ValidationService
    {
        public static void IsUserReview(UserReviewRequest request)
        {
            if (request.Score is > 5 or < 1)
                throw new BadRequestException("Оценка отзыва должна быть больше 1 и меньше 5");
            if (request.Title is null || request.Title.Length > 20 || request.Title.Length < 5)
                throw new BadRequestException("Длина заголовка должна находится между 5 и 20 символами");
            if (request.Content is null || request.Content.Length > 50 || request.Content.Length < 5)
                throw new BadRequestException("Длина описания должна находится между 5 и 50 символами");
        }
    }
}
