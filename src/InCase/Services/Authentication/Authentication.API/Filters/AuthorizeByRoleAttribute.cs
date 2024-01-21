using Microsoft.AspNetCore.Authorization;

namespace Authentication.API.Filters;

public class AuthorizeByRoleAttribute : AuthorizeAttribute
{
    public AuthorizeByRoleAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}