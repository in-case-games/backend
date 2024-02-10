using Promocode.DAL.Entities;

namespace Promocode.BLL.Models;
public class UserPromoCodeResponse : BaseEntity
{
    public DateTime Date { get; set; }
    public bool IsActivated { get; set; }
    public string? Name { get; set; }
    public int? Discount { get; set; }
    public string? Type { get; set; }
}