using System.Text.Json.Serialization;

namespace Infrastructure.MassTransit.Resources
{
    public class LootBoxBannerTemplate : BaseTemplate
    {
        public DateTime CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsDeleted { get; set; }

        public Guid BoxId { get; set; }
    }
}
