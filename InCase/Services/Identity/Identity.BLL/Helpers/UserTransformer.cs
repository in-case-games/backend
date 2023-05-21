using Identity.BLL.Models;
using Identity.DAL.Entities;

namespace Identity.BLL.Helpers
{
    public static class UserTransformer
    {
        public static UserResponse ToResponse(this User user) =>
            new()
            {
                Login = user.Login,
                Email = user.Email,
                Balance = user.AdditionalInfo!.Balance,
                ImageUri = user.AdditionalInfo.ImageUri
            };

        public static List<UserResponse> ToResponse(this List<User> users)
        {
            List<UserResponse> usersResponses = new();

            foreach (User user in users)
                usersResponses.Add(user.ToResponse());

            return usersResponses;
        }

    }
}