using System.Text.Json.Serialization;

namespace Review.DAL.Entities ;

public class User : BaseEntity
{
    [JsonIgnore]
    public IEnumerable<UserReview>? Reviews { get; set; }
}