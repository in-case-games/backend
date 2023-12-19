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

        public static List<AnswerImageResponse> ToResponse(this IEnumerable<AnswerImage> entities)
        {
            var response = new List<AnswerImageResponse>();

            foreach (var entity in entities) response.Add(ToResponse(entity));

            return response;
        }
    }
}
