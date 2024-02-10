using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Promocode.DAL.Entities;
public class PromoCodeType : BaseEntity
{
    [MaxLength(50)]
    public string? Name { get; set; }

    [JsonIgnore]
    public PromoCode? PromoCode { get; set; }
}