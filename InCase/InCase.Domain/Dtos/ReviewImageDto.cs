using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class ReviewImageDto : BaseEntity
    {
        public string? Uri { get; set; } = "";
        public Guid ReviewId { get; set; }

        public ReviewImage Convert() => new()
        {
            Uri = Uri,
            ReviewId = ReviewId,
        };
    }
}
