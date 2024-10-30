using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.User;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface IUserService
    {
        BaseResponse GetAllUser();

        BaseResponse GetUser(int id);

        BaseResponse CreateUser(UserRequest user);

        BaseResponse UpdateUser(int id, UserRequest user);

        BaseResponse DeleteUser(int id);
    }
}
