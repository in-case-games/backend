using Authentication.DAL.Entities;
using System.Text.Json.Serialization;

namespace Authentication.BLL.Models
{
    public class UserRestrictionRequest : BaseEntity
    {
        public DateTime ExpirationDate { get; set; }

        public Guid UserId { get; set; }
    }
}
