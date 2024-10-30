using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Services.Interfaces
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
