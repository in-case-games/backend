using Game.BLL.Models;
using Game.DAL.Entities;

namespace Game.BLL.Helpers;
public static class UserOpeningTransformer
{
    public static List<UserOpeningResponse> ToResponse(this List<UserOpening> openings) =>
        openings.Select(opening => new UserOpeningResponse
            {
                BoxId = opening.BoxId,
                Date = opening.Date,
                Id = opening.Id,
                ItemId = opening.ItemId,
                UserId = opening.UserId,
            })
            .ToList();
}