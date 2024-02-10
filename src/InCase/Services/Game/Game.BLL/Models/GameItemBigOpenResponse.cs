using Game.DAL.Entities;

namespace Game.BLL.Models;
public class GameItemBigOpenResponse : BaseEntity
{
    public int Count { get; set; }
    public decimal Cost { get; set; }
}