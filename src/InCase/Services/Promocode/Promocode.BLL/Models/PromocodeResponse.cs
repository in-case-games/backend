using Promocode.DAL.Entities;

namespace Promocode.BLL.Models;
public class PromoCodeResponse : BaseEntity
{
    public string? Name { get; set; }
    public decimal Discount { get; set; }
    public int NumberActivations { get; set; }
    public DateTime ExpirationDate { get; set; }
    public PromoCodeType? Type { get; set; }
}