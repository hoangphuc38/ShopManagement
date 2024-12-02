using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.User;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse> GetAllUser();

        Task<BaseResponse> GetUser(int id);

        Task<BaseResponse> UpdateUser(int id, UserRequest user);

        Task<BaseResponse> DeleteUser(int id);
    }
}
