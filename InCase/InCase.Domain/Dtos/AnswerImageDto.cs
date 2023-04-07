using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;

namespace InCase.Domain.Dtos
{
    public class AnswerImageDto : BaseEntity
    {
        public string? ImageUri { get; set; } = "";

        public Guid AnswerId { get; set; }

        public AnswerImage Convert() => new()
        {
            Id = Id,
            ImageUri = ImageUri,
            AnswerId = AnswerId
        };
    }
}
