using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Promocode.DAL.Entities;
public class PromoCode : BaseEntity
{
    [MaxLength(50)]
    public string? Name { get; set; }
    public decimal Discount { get; set; }
    public int NumberActivations { get; set; }
    public DateTime ExpirationDate { get; set; }

    public PromoCodeType? Type { get; set; }

    [JsonIgnore]
    public Guid TypeId { get; set; }
    [JsonIgnore]
    public IEnumerable<UserPromoCode>? HistoriesPromoCodes { get; set; }
}