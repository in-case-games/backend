using Support.BLL.Models;
using Support.DAL.Entities;

namespace Support.BLL.Helpers
{
    public static class SupportTopicAnswerTransformer
    {
        public static SupportTopicAnswer ToEntity(
            this SupportTopicAnswerRequest request, 
            bool isNewGuid = false) => new()
        {
            Id = isNewGuid ? Guid.NewGuid() : request.Id,
            Content = request.Content,
            Date = request.Date,
            PlaintiffId = request.PlaintiffId,
            TopicId = request.TopicId,
        };

        public static SupportTopicAnswerResponse ToResponse(this SupportTopicAnswer entity) => new()
        {
            Id = entity.Id,
            Content = entity.Content,
            Date = entity.Date,
            Images = entity.Images?.ToResponse(),
            PlaintiffId = entity.PlaintiffId,
            TopicId = entity.TopicId,
        };

        public static List<SupportTopicAnswerResponse> ToResponse(this IEnumerable<SupportTopicAnswer> entities)
        {
            List<SupportTopicAnswerResponse> response = new();

            foreach (var entity in entities)
                response.Add(ToResponse(entity));

            return response;
        }
    }
}
