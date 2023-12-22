using Support.BLL.Models;
using Support.DAL.Entities;

namespace Support.BLL.Helpers
{
    public static class SupportTopicAnswerTransformer
    {
        public static SupportTopicAnswer ToEntity(this SupportTopicAnswerRequest request, bool isNewGuid = false) => 
            new()
            {
                Id = isNewGuid ? Guid.NewGuid() : request.Id,
                Content = request.Content,
                Date = request.Date,
                PlaintiffId = request.PlaintiffId,
                TopicId = request.TopicId,
            };

        public static SupportTopicAnswerResponse ToResponse(this SupportTopicAnswer entity) => 
            new()
            {
                Id = entity.Id,
                Content = entity.Content,
                Date = entity.Date,
                Images = entity.Images?.ToResponse(),
                PlaintiffId = entity.PlaintiffId,
                TopicId = entity.TopicId,
            };

        public static List<SupportTopicAnswerResponse> ToResponse(this IEnumerable<SupportTopicAnswer> entities) =>
            entities.Select(ToResponse).ToList();

        public static List<AnswerImageResponse> ToAnswerImageResponse(this IEnumerable<SupportTopicAnswer> answers)
        {
            var response = new List<AnswerImageResponse>();

            foreach (var answer in answers) response.AddRange(answer.Images!.ToResponse());

            return response;
        }
    }
}
