using Authentication.DAL.Entities;

namespace Authentication.BLL.Models;
public class UserResponse : BaseEntity
{
    public string? Email { get; set; }
    public string? Login { get; set; }
}