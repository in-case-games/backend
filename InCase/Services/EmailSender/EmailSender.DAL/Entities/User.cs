using System.Text.Json.Serialization;

namespace EmailSender.DAL.Entities
{
    public class User : BaseEntity
    {
        [JsonIgnore]
        public UserAdditionalInfo? AdditionalInfo { get; set; }
    }
}
