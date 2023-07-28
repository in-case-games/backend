using Game.BLL.Models;
using Game.DAL.Entities;

namespace Game.BLL.Helpers
{
    public static class UserOpeningTransformer
    {
        public static UserOpeningResponse ToResponse(this UserOpening opening) => new()
        {
            BoxId = opening.BoxId,
            Date = opening.Date,
            Id = opening.Id,
            ItemId = opening.ItemId,
            UserId = opening.UserId,
        };


        public static List<UserOpeningResponse> ToResponse(this List<UserOpening> openings)
        {
            List<UserOpeningResponse> response = new();

            foreach (var opening in openings)
                response.Add(ToResponse(opening));

            return response;
        }
    }
}
