using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class NewsImageDto : BaseEntity
    {
        public string? Uri { get; set; }
        public Guid NewsId { get; set; }

        public NewsImage Convert() => new()
        {
            Uri = Uri,
            NewsId = NewsId,
        };
    }
}
