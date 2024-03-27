using Microsoft.AspNetCore.Authorization;

namespace Support.API.Filters;
public class AuthorizeByRoleAttribute : AuthorizeAttribute
{
    public AuthorizeByRoleAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}