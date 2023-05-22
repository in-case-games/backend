using Resources.DAL.Entities;
using System.Text.Json.Serialization;

namespace Resources.BLL.Models
{
    public class LootBoxRequest
    {
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public Guid GameId { get; set; }
    }
}
