using System.Text.Json.Serialization;
using Payment.BLL.Models.External;
using Payment.DAL.Entities;

namespace Payment.BLL.Models.Internal;
public class InvoiceUrlRequest
{
    public Amount? Amount { get; set; }

    [JsonIgnore]
    public User? User { get; set; }
}