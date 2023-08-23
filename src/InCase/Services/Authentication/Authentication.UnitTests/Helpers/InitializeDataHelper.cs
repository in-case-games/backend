using Authentication.BLL.Services;
using Authentication.DAL.Data;
using Authentication.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.UnitTests.Helpers
{
    public static class InitializeDataHelper
    {
        private static string GenerateString(int length = 8)
        {
            Random random = new();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static async Task<User> InitialiseUser(this ApplicationDbContext context,
            Guid guid,
            string roleName = "user",
            string password = "password",
            bool isBanned = false)
        {
            UserRole? userRole = await context.Roles
                .FirstOrDefaultAsync(x => x.Name == roleName);

            UserAdditionalInfo? info = new()
            {
                IsConfirmed = true,
                RoleId = userRole!.Id,
                Role = userRole,
                UserId = guid
            };

            User? user = new()
            {
                Id = guid,
                Login = $"{GenerateString()}UserApiTest",
                Email = $"{GenerateString()}UserApiTest@mail.ru",
                AdditionalInfo = info
            };

            AuthenticationService.CreateNewPassword(ref user, password);

            await context.Users.AddAsync(user);
            await context.AdditionalInfos.AddAsync(info);

            UserRestriction userRestriction = new UserRestriction()
            {
                UserId = guid,
                ExpirationDate = DateTime.Now.AddDays(30)
            };
            if (isBanned)
            {
                await context.Restrictions.AddAsync(userRestriction);
            }

            await context.SaveChangesAsync();
            context.Entry(user).State = EntityState.Detached;
            context.Entry(info).State = EntityState.Detached;
            context.Entry(userRestriction).State = EntityState.Detached;
            context.Entry(userRole).State = EntityState.Detached;

            return user;
        }
    }
}
