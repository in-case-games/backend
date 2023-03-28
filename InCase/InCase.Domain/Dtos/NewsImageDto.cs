using InCase.Domain.Entities;
using System.Text.Json.Serialization;

namespace InCase.Domain.Dtos
{
    public class NewsImageDto : BaseEntity
    {
        public string? Uri { get; set; }
        public Guid NewsId { get; set; }
    }
}
