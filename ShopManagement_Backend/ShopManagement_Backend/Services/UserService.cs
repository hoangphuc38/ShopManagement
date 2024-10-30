using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Repositories.Interfaces;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;
using ShopManagement_Backend.Services.Interfaces;

namespace ShopManagement_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IShopRepository _shopRepo;
        private readonly IShopDetailRepository _shopDetailRepo;

        public UserService(
            IMapper mapper,
            IUserRepository userRepo,
            IShopRepository shopRepo,
            IShopDetailRepository shopDetailRepo)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _shopRepo = shopRepo;
            _shopDetailRepo = shopDetailRepo;
        }

        public BaseResponse GetAllUser()
        {
            var userList = _userRepo.GetAllAsync(c => c.IsDeleted == false);
            var responseList = _mapper.Map<List<UserResponse>>(userList);

            return new BaseResponse(responseList);
        }

        public BaseResponse GetUser(int id)
        {
            var user = _userRepo.GetFirstAsync(c => c.Id == id);

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

            _userRepo.AddAsync(newUser);

            return new BaseResponse("Create user successfully");
        }

        public BaseResponse UpdateUser(int id, UserRequest user)
        {
            var userUpdate = _userRepo.GetFirstAsync(c => c.Id == id);

            if (userUpdate == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
            }

            userUpdate.FullName = user.FullName;
            userUpdate.UserName = user.UserName;
            userUpdate.Address = user.Address;

            _userRepo.UpdateAsync(userUpdate);

            var response = _mapper.Map<UserResponse>(userUpdate);

            return new BaseResponse("Update user successfully");
        }

        public BaseResponse DeleteUser(int id)
        {
            var user = _userRepo.GetFirstAsync(c => c.Id == id);

            if (user == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
            }

            user.IsDeleted = true;
            _userRepo.UpdateAsync(user);

            var shopList = _shopRepo.GetAllAsync(c => c.UserId == user.Id);
            foreach (var shop in shopList)
            {
                shop.IsDeleted = true;
                _shopRepo.UpdateAsync(shop);

                var shopDetail = _shopDetailRepo.GetAllAsync(c => c.ShopId == shop.ShopId);
                foreach (var detail in shopDetail)
                {
                    detail.IsDeleted = true;
                    _shopDetailRepo.DeleteAsync(detail);
                }
            }

            return new BaseResponse("Delete user successfully");
        }
    }
}
