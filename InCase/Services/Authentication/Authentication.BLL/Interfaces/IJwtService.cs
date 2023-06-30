﻿using Authentication.DAL.Entities;
using System.Security.Claims;

namespace Authentication.BLL.Interfaces
{
    public interface IJwtService
    {
        public ClaimsPrincipal GetClaimsToken(string token);
        public string CreateEmailToken(in User user);
        public TokensResponse CreateTokenPair(in User user);
    }
}