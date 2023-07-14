using Authentication.DAL.Entities;

namespace Authentication.BLL.MassTransit.Models
{
    public class UserTemplate : BaseEntity
    {
        public string? Email { get; set; }
        public string? Login { get; set; }
        public bool IsDeleted { get; set; }
    }
}
