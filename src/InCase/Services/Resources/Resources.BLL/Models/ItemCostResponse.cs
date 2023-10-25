using System.Text.Json.Serialization;

namespace Resources.BLL.Models
{
    public class ItemCostResponse
    {
        public bool Success { get; set; } = false;
        public decimal Cost { get; set; }
    }
}
