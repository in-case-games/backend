using Resources.DAL.Entities;
using System.Text.Json.Serialization;

namespace Resources.BLL.Models
{
    public class LootBoxRequest : BaseEntity
    {
        public string? Name { get; set; }
        public string? HashName { get; set; }
        public string? IdForMarket { get; set; }
        public decimal Cost { get; set; }
        public Guid GameId { get; set; }
    }
}
