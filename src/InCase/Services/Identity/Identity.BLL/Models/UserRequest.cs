using Identity.DAL.Entities;

namespace Identity.BLL.Models
{
    public class UserRequest : BaseEntity
    {
        public string? Login { get; set; }
    }
}
