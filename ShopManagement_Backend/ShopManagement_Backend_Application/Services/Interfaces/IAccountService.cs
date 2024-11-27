using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Token;
using ShopManagement_Backend_Application.Models.User;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface IAccountService
    {
        BaseResponse Register(RegisterUser registerUser);

        BaseResponse Login(LoginUser loginUser);

        BaseResponse RefreshToken(RefreshTokenRequest request);

        BaseResponse AddRole(string role);

        BaseResponse AssignRole(string email, string role);

        BaseResponse Logout(string refreshToken);
    }
}
