using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Service
{
    public class UserService
    {
        private readonly ShopManagementDbContext _context;
        private readonly IMapper _mapper;

        public UserService(ShopManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BaseResponse GetAllUser()
        {
            var userList = _context.Users.Where(c => c.IsDeleted == false).ToList();
            var responseList = new List<UserResponse>();

            foreach (var user in userList)
            {
                var response = _mapper.Map<UserResponse>(user);

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
            
            var response = _mapper.Map<UserResponse>(user);

            return new BaseResponse(response);
        }

        public BaseResponse CreateUser(UserRequest user)
        {
            var newUser = _mapper.Map<User>(user);
            newUser.IsDeleted = false;
            newUser.SignUpDate = DateOnly.FromDateTime(DateTime.Now);

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

            var response = _mapper.Map<UserResponse>(userUpdate);

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
