namespace Withdraw.DAL.Entities
{
    public class Game : BaseEntity
    {
        public string? Name { get; set; }

        public IEnumerable<GameItem>? Items { get; set; }
        public IEnumerable<GameMarket>? Markets { get; set; }
    }
}
