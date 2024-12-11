using Microsoft.AspNetCore.Identity.Data;
using ShopManagement_Backend_Application.Models.Token;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Core.Entities;

namespace ShopManagement.Api.Test.Fixtures
{
    public class AccountTest
    {
        public RegisterUser register = new RegisterUser
        {
            UserName = "Test@gmail.com",
            Password = "djajskdhsjkd",
            FullName = "Test",
            Address = "AbC",
            Role = "test",
        };

        public LoginUser loginRequest = new LoginUser
        {
            Email = "test@gmail.com",
            Password = "adhkashdkad"
        };

        public LoginResponseModel loginRes = new LoginResponseModel
        {
            FullName = "Test",
            RefreshToken = "dhasdhkadhskjad",
            UserName = "test@gmail.com",
            Role = "admin",
            Token = "fkjasdhkjdkjad"
        };

        public RefreshTokenRequest tokenRequest = new RefreshTokenRequest
        {
            RefreshToken = "djshkadhskad"
        };

        public RefreshTokenResponse tokenResponse = new RefreshTokenResponse
        {
            NewAccessToken = "dhjasjhdkasdhk"
        };

        public Token token = new Token
        {
            TokenId = 1,
            RefreshToken = "hadkajdhjasdhak",
            ExpiredDate = DateTime.Now.AddDays(5),
            UserID = 1,
        };

        public Token tokenNull = null;
    }
}
