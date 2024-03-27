using Microsoft.AspNetCore.Authorization;

namespace Promocode.API.Filters;
public class AuthorizeByRoleAttribute : AuthorizeAttribute
{
    public AuthorizeByRoleAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}