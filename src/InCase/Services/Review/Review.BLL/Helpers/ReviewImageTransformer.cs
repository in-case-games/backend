using Review.BLL.Models;
using Review.DAL.Entities;

namespace Review.BLL.Helpers
{
    public static class ReviewImageTransformer
    {
        public static ReviewImageResponse ToResponse(this ReviewImage entity) => new()
        {
            Id = entity.Id,
            ReviewId = entity.ReviewId,
        };

        public static List<ReviewImageResponse> ToResponse(this List<ReviewImage> entities)
        {
            var response = new List<ReviewImageResponse>();

            foreach (var entity in entities) response.Add(ToResponse(entity));

            return response;
        }

        public static List<ReviewImageResponse> ToResponse(this IEnumerable<ReviewImage> entities)
        {
            var response = new List<ReviewImageResponse>();

            foreach (var entity in entities) response.Add(ToResponse(entity));

            return response;
        }
    }
}
