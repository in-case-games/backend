using Support.BLL.Models;
using Support.DAL.Entities;

namespace Support.BLL.Helpers
{
    public static class SupportTopicTransformer
    {
        public static SupportTopic ToEntity(this SupportTopicRequest request, bool isNewGuid = false) => new()
        {
            Id = isNewGuid ? Guid.NewGuid() : request.Id,
            Content = request.Content,
            Date = request.Date,
            IsClosed = request.IsClosed,
            Title = request.Title,
            UserId = request.UserId
        };

        public static SupportTopicResponse ToResponse(this SupportTopic entity) => new()
        {
            Id = entity.Id,
            Answers = entity.Answers?.ToResponse(),
            Content = entity.Content,
            Date = entity.Date,
            IsClosed = entity.IsClosed,
            Title = entity.Title,
            UserId = entity.UserId
        };

        public static List<SupportTopicResponse> ToResponse(this List<SupportTopic> entities)
        {
            var response = new List<SupportTopicResponse>();

            foreach (var entity in entities) response.Add(entity.ToResponse());

            return response;
        }
    }
}
