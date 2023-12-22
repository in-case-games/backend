using Review.BLL.Models;
using Review.DAL.Entities;

namespace Review.BLL.Helpers
{
    public static class UserReviewTransformer
    {
        public static UserReview ToEntity(this UserReviewRequest request, bool isNewGuid = false) => new()
        {
            Id = isNewGuid ? Guid.NewGuid() : request.Id,
            Content = request.Content,
            CreationDate = request.CreationDate,
            IsApproved = request.IsApproved,
            Score = request.Score,
            Title = request.Title,
            UserId = request.UserId,
        };

        public static UserReviewResponse ToResponse(this UserReview entity) => new()
        {
            Id = entity.Id,
            IsApproved = entity.IsApproved,
            Content = entity.Content,
            CreationDate = entity.CreationDate,
            Images = entity.Images?.ToResponse(),
            Score = entity.Score,
            Title = entity.Title,
            UserId = entity.UserId,
        };

        public static List<UserReviewResponse> ToResponse(this List<UserReview> entities) =>
            entities.Select(ToResponse).ToList();
    }
}
