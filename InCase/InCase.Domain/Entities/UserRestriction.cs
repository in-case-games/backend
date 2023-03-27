using System.Text.Json.Serialization;
using InCase.Domain.Dtos;

namespace InCase.Domain.Entities
{
    public class UserRestriction : BaseEntity
    {
        public string? Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Description { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        public UserRestrictionDto Convert() => new()
        {
            Name = Name,
            CreationDate = CreationDate,
            ExpirationDate = ExpirationDate,
            Description = Description,
            UserId = User?.Id ?? UserId
        };
    }
}
