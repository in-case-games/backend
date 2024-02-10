using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Helpers;
public static class GameTransformer
{
    public static GameResponse ToResponse(this Game game) =>
        new()
        {
            Id = game.Id,
            Name = game.Name,
            Boxes = game.Boxes?.ToResponse(),
        };

    public static List<GameResponse> ToResponse(this List<Game> games) =>
        games.Select(ToResponse).ToList();
}