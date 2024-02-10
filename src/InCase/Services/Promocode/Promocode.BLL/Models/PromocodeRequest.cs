using Promocode.DAL.Entities;

namespace Promocode.BLL.Models;
public class PromoCodeRequest : BaseEntity
{
    public string? Name { get; set; }
    public decimal Discount { get; set; }
    public int NumberActivations { get; set; }
    public DateTime ExpirationDate { get; set; }
    public Guid TypeId { get; set; }
}