using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Token;
using ShopManagement_Backend_Application.Models.User;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse> Register(RegisterUser registerUser);

        Task<BaseResponse> Login(LoginUser loginUser);

        Task<BaseResponse> RefreshToken(RefreshTokenRequest request);

        Task<BaseResponse> AddRole(string role);

        Task<BaseResponse> Logout(int userID);
    }
}
