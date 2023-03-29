using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class ReviewImageDto : BaseEntity
    {
        public string? ImageUri { get; set; } = "";
        public Guid ReviewId { get; set; }

        public ReviewImage Convert() => new()
        {
            ImageUri = ImageUri,
            ReviewId = ReviewId,
        };
    }
}
