using Game.DAL.Entities;
using System.Text.Json.Serialization;

namespace Game.BLL.Models
{
    public class UserOpeningResponse : BaseEntity
    {
        public DateTime Date { get; set; }

        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
        public Guid BoxId { get; set; }
    }
}
