using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class AuthenticationTestHelper
    {
        private readonly ResponseHelper _responseHelper = new();

        private readonly User SuperAdminModel = new() { 
            UserEmail = "yt_ferbray@mail.ru",
            UserLogin = "GIS"
        };
        private readonly string Password = "1234";
        private TokenModel? SuperAdminTokens;

        public async Task<TokenModel> SignInUser(User user, string ip)
        {
            await _responseHelper.ResponsePostStatusCode(
                $"/Authentication/SignUp?" +
                $"password={Password}", user);
            await _responseHelper.ResponsePostStatusCode($"/UserAdditionalInfo", user);

            TokenModel? tokenModel = await _responseHelper.ResponsePost<User, TokenModel?>(
                $"/Authentication/SignIn?" +
                $"password={Password}&ip={ip}", user);

            if (tokenModel == null) throw new Exception("Error singin it`s null:)");

            return tokenModel;
        }

        public async Task<TokenModel> SignInAdmin(User user, string ip)
        {
            //Create User
            await _responseHelper.ResponsePostStatusCode(
                $"/Authentication/SignUp?" +
                $"password={Password}", user);
            await _responseHelper.ResponsePostStatusCode($"/UserAdditionalInfo", user);

            //Get 2 tokens(user, super admin)
            TokenModel? tokenModel = await _responseHelper.ResponsePost<User, TokenModel?>(
                $"/Authentication/SignIn?" +
                $"password={Password}&ip={ip}", user);

            TokenModel? tokenModelAdmin = await _responseHelper.ResponsePost<User, TokenModel?>(
                $"/Authentication/SignIn?" +
                $"password={Password}&ip={ip}", SuperAdminModel);

            if (tokenModel == null) throw new Exception("Error singin it`s null:)");
            if (tokenModelAdmin == null) throw new Exception("Error singin admin it`s null:)");

            //Change role in admin
            UserAdditionalInfo? userInfo = await _responseHelper.ResponseGet<UserAdditionalInfo?>(
                $"/UserAdditionalInfo", 
                tokenModel.AccessToken!);
            UserRole? userRole = await _responseHelper.ResponseGet<UserRole?>("/Role?name=admin");

            userInfo!.UserRoleId = userRole!.Id;
            userInfo!.UserBalance = 9999999999M;

            await _responseHelper.ResponsePut(
                $"/UserAdditionalInfo/UpdateInfoByAdmin", 
                userInfo, 
                tokenModelAdmin.AccessToken!);

            //Save super admin tokens
            SuperAdminTokens = tokenModelAdmin;
            
            return tokenModel;
        }

        public async Task<bool> DeleteUserByAdmin(string login)
        {
            User? user = await _responseHelper.ResponseGet<User?>($"/User/GetByLogin?" +
                $"login={login}");

            if(user == null) throw new Exception("Error no such user:)");

            HttpStatusCode code = await _responseHelper.ResponseDelete(
                $"/User/DeleteByAdmin?userId={user.Id}",
                SuperAdminTokens!.AccessToken!);

            return code == HttpStatusCode.OK;
        }

        public async Task<TokenModel> RefreshTokens(string refreshToken, string ip)
        {
            TokenModel? tokenModel = await _responseHelper.ResponseGet<TokenModel>(
                $"/Authentication/RefreshTokens?" +
                $"refreshToken={refreshToken}&" +
                $"ip={ip}");

            if (tokenModel == null) throw new Exception("Error refresh it`s null:)");

            return tokenModel;
        }

        public async Task<bool> Logout(TokenModel tokenModel)
        {
            HttpStatusCode code = await _responseHelper.ResponseDelete(
                $"/Authentication/Logout?" +
                $"refreshToken={tokenModel.RefreshToken}", 
                tokenModel.AccessToken!);

            return code == HttpStatusCode.OK;
        }
    }
}
