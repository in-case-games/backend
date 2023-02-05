using CaseApplication.Api.Models;
using CaseApplication.DomainLayer.Entities;
using CaseApplication.WebClient.Services;
using System.Net;

namespace CaseApplication.IntegrationTests.Api
{
    public class Authentication
    {
        private readonly ResponseHelper _responseHelper = new();

        private readonly User AdminModel = new() { 
            UserEmail = "yt_ferbray@mail.ru",
            UserLogin = "GIS"
        };
        private readonly User UserModel = new()
        {
            UserEmail = "yt_xy@mail.ru",
            UserLogin = "GyS"
        };
        private readonly string Password = "1234";

        public async Task<TokenModel> SignInUser(string ip)
        {
            await _responseHelper.ResponsePostStatusCode(
                $"/Authentication/SignUp?" +
                $"password={Password}", UserModel);

            TokenModel? tokenModel = await _responseHelper.ResponsePost<User, TokenModel>(
                $"/Authentication/SignIn?" +
                $"password={Password}&ip={ip}", UserModel);

            if (tokenModel == null) throw new Exception("Error singin it`s null:)");

            return tokenModel;
        }
        public async Task<TokenModel> SignInAdmin(string ip)
        {
            await _responseHelper.ResponsePostStatusCode(
                $"/Authentication/SignUp?" +
                $"password={Password}", AdminModel);

            TokenModel? tokenModel = await _responseHelper.ResponsePost<User, TokenModel>(
                $"/Authentication/SignIn?" +
                $"password={Password}&ip={ip}", AdminModel);

            if (tokenModel == null) throw new Exception("Error singin it`s null:)");

            return tokenModel;
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
