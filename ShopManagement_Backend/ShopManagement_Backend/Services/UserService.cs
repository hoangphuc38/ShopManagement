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

        public IEnumerable<UserResponse> GetAllUser()
        {
            var userList = _context.Users.ToList();
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

            return responseList;
        }

        public UserResponse? GetUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return null;
            }

            return new UserResponse
            {
                UserID = id,
                UserName = user.UserName,
                FullName = user.FullName,
                Address = user.Address ?? "",
                SignUpDate = user.SignUpDate,
            };
        }

        public UserResponse? UpdateUser(int id, UserRequest user)
        {
            var userUpdate = _context.Users.Find(id);

            if (userUpdate == null)
            {
                return null;
            }

            userUpdate.FullName = user.FullName;
            userUpdate.UserName = user.UserName;
            userUpdate.Address = user.Address;

            _context.Update(userUpdate);
            _context.SaveChanges();

            return new UserResponse
            {
                UserID = id,
                UserName = userUpdate.UserName,
                FullName = userUpdate.FullName,
                Address = userUpdate.Address,
                SignUpDate = userUpdate.SignUpDate,
            };
        }
    }
}
