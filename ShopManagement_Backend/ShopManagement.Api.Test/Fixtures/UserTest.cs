using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Core.Entities;

namespace ShopManagement.Api.Test.Fixtures
{
    public class UserTest
    {
        public PaginationResponse UserPagination = new PaginationResponse
        {
            PageNumber = 1,
            PageSize = 10,
            TotalOfPages = 1,
            TotalOfNumberRecord = 1,
            Results = new List<UserResponse>
            {
                new UserResponse
                {
                    UserID = 1,
                    UserName = "Test@example.com",
                    FullName = "Test",
                    Address = "ABC",
                    SignUpDate = DateTime.Now,
                }
            },
        };

        public UserResponse userRes = new UserResponse
        {
            UserID = 1,
            UserName = "Test@example.com",
            FullName = "Test",
            Address = "ABC",
            SignUpDate = DateTime.Now,
        };

        public User user = new User
        {
            Id = 1,
            FullName = "Test",
            UserName = "test@gmail.com",
            Address = "abc",
            SignUpDate = DateTime.Now,
            RoleId = 1,
            IsDeleted = false,
        };

        public List<User> userList = new List<User>
        {
            new User
            {
                Id = 1,
                FullName= "Test",
                UserName = "test@gmail.com",
                Address = "abc",
                SignUpDate= DateTime.Now,
                RoleId = 1,
                IsDeleted = false,
            },
            new User
            {
                Id = 2,
                FullName= "Test",
                UserName = "test@gmail.com",
                Address = "abc",
                SignUpDate= DateTime.Now,
                RoleId = 1,
                IsDeleted = false,
            },
        };
    }
}
