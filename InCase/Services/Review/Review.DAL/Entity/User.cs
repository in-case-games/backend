using System.Text.Json.Serialization;

namespace Review.DAL.Entity
{
    public class User : BaseEntity
    {
        [JsonIgnore]
        public IEnumerable<UserReview>? Reviews { get; set; }
    }
}
