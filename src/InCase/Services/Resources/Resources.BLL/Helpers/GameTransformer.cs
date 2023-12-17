using Resources.BLL.Models;
using Resources.DAL.Entities;

namespace Resources.BLL.Helpers
{
    public static class GameTransformer
    {
        public static GameResponse ToResponse(this Game game) =>
            new()
            {
                Id = game.Id,
                Name = game.Name,
                Boxes = game.Boxes?.ToResponse(),
            };

        public static List<GameResponse> ToResponse(this List<Game> games)
        {
            var result = new List<GameResponse>();

            foreach (var game in games) result.Add(ToResponse(game));

            return result;
        }
    }
}
