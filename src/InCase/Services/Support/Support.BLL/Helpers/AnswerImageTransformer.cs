using Support.BLL.Models;
using Support.DAL.Entities;

namespace Support.BLL.Helpers
{
    public static class AnswerImageTransformer
    {
        public static AnswerImageResponse ToResponse(this AnswerImage entity) => new()
        {
            Id = entity.Id,
            AnswerId = entity.AnswerId
        };

        public static List<AnswerImageResponse> ToResponse(this IEnumerable<AnswerImage> entities) =>
            entities.Select(ToResponse).ToList();
    }
}
