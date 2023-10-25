using Support.BLL.Models;
using Support.DAL.Entities;

namespace Support.BLL.Helpers
{
    public static class AnswerImageTransformer
    {
        public static AnswerImage ToEntity(this AnswerImageRequest request, bool isNewGuid = false) => new()
        {
            Id = isNewGuid ? Guid.NewGuid() : request.Id,
            AnswerId = request.AnswerId,
        };

        public static AnswerImageResponse ToResponse(this AnswerImage entity) => new()
        {
            Id = entity.Id,
            AnswerId = entity.AnswerId
        };

        public static List<AnswerImageResponse> ToResponse(this IEnumerable<AnswerImage> entities)
        {
            List<AnswerImageResponse> response = new();

            foreach (var entity in entities)
                response.Add(ToResponse(entity));

            return response;
        }
    }
}
