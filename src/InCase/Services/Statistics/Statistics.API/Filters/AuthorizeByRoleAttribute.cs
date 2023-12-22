using Microsoft.AspNetCore.Authorization;

namespace Statistics.API.Filters
{
    public class AuthorizeByRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeByRoleAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}
