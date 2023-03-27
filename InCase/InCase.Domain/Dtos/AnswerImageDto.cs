using InCase.Domain.Entities;
using InCase.Domain.Entities.Resources;
using System.Text.Json.Serialization;

namespace InCase.Domain.Dtos
{
    public class AnswerImageDto : BaseEntity
    {
        public string? ImageUri { get; set; } = "";

        public Guid AnswerId { get; set; }

        public AnswerImageDto Convert() => new()
        {
            ImageUri = ImageUri,
            AnswerId = AnswerId
        };
    }
}
