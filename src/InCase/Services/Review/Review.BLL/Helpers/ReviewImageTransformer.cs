using Review.BLL.Models;
using Review.DAL.Entities;

namespace Review.BLL.Helpers;
public static class ReviewImageTransformer
{
    public static ReviewImageResponse ToResponse(this ReviewImage entity) => new()
    {
        Id = entity.Id,
        ReviewId = entity.ReviewId,
    };

    public static List<ReviewImageResponse> ToResponse(this List<ReviewImage> entities) =>
        entities.Select(ToResponse).ToList();

    public static List<ReviewImageResponse> ToResponse(this IEnumerable<ReviewImage> entities) =>
        entities.Select(ToResponse).ToList();
}