using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.User;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface IAccountService
    {
        BaseResponse Register(RegisterUser registerUser);

        BaseResponse Login(LoginUser loginUser);

        BaseResponse AddRole(string role);

        BaseResponse AssignRole(string email, string role);
    }
}
