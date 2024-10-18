using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Service
{
    public class UserService
    {
        private readonly ShopManagementDbContext _context;

        public UserService(ShopManagementDbContext context)
        {
            _context = context;
        }

        public BaseResponse GetAllUser()
        {
            var userList = _context.Users.Where(c => c.IsDeleted == false).ToList();
            var responseList = new List<UserResponse>();

            foreach (var user in userList)
            {
                UserResponse response = new UserResponse
                {
                    UserID = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Address = user.Address ?? "",
                    SignUpDate = user.SignUpDate,
                };

                responseList.Add(response);
            }

            return new BaseResponse(responseList);
        }

        public BaseResponse GetUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
            }

            UserResponse response = new UserResponse
            {
                UserID = id,
                UserName = user.UserName,
                FullName = user.FullName,
                Address = user.Address ?? "",
                SignUpDate = user.SignUpDate,
            };

            return new BaseResponse(response);
        }

        public BaseResponse CreateUser(UserRequest user, string password)
        {
            User newUser = new User
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Password = password,
                Address = user.Address,
                IsDeleted = false,
                SignUpDate = DateOnly.FromDateTime(DateTime.Now),
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return new BaseResponse("Create user successfully");
        }

        public BaseResponse UpdateUser(int id, UserRequest user)
        {
            var userUpdate = _context.Users.Find(id);

            if (userUpdate == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
            }

            userUpdate.FullName = user.FullName;
            userUpdate.UserName = user.UserName;
            userUpdate.Address = user.Address;

            _context.Update(userUpdate);
            _context.SaveChanges();

            UserResponse response = new UserResponse
            {
                UserID = id,
                UserName = userUpdate.UserName,
                FullName = userUpdate.FullName,
                Address = userUpdate.Address,
                SignUpDate = userUpdate.SignUpDate,
            };

            return new BaseResponse(response);
        }

        public BaseResponse DeleteUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
            }

            user.IsDeleted = true;
            _context.Update(user);
            _context.SaveChanges();

            return new BaseResponse("Delete user successfully");
        }
    }
}
