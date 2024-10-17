using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;
using ShopManagement_Backend.Service;

namespace ShopManagement_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _repo;

        public UserController(UserService repo)
        {
            _repo = repo;
        }

        [HttpGet("get-all")]
        public ActionResult<BaseResponse<IEnumerable<UserResponse>>> GetAllUser()
        {
            var userList = _repo.GetAllUser();

            return new BaseResponse<IEnumerable<UserResponse>>
            {
                Success = true,
                Message = "Get all users successfully",
                Data = userList,
            };
        }

        [HttpGet("get-by-id/{id}")]
        public ActionResult<BaseResponse<UserResponse>> GetUser(int id)
        {
            var user = _repo.GetUser(id);

            if (user == null)
            {
                return new BaseResponse<UserResponse>
                {
                    Success = false,
                    Message = "User not found",
                    Data = null
                };
            }

            return new BaseResponse<UserResponse>
            {
                Success = true,
                Message = "Get user's information successfully",
                Data = user
            };
        }

        [HttpPut("update/{id}")]
        public ActionResult<BaseResponse<UserResponse>> UpdateUser(int id, UserRequest user)
        {
            var userUpdate = _repo.UpdateUser(id, user);

            if (userUpdate == null)
            {
                return new BaseResponse<UserResponse>
                {
                    Success = false,
                    Message = "User not found",
                    Data = null
                };
            }

            return new BaseResponse<UserResponse>
            {
                Success = true,
                Message = "Update user's information successfully",
                Data = userUpdate
            };
        }
    }
}
