using Authentication.DAL.Entities;

namespace Authentication.BLL.Models
{
    public class UserRequest : BaseEntity
    {
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
