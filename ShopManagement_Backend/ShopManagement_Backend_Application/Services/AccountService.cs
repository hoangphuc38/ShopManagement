using Azure.Core;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.Helpers;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Token;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Identity;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly JwtHelper _jwtHelper;
        private readonly IUserRepository _userRepo;
        private readonly ITokenRepository _tokenRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            JwtHelper jwtHelper,
            IUserRepository userRepo,
            ITokenRepository tokenRepo,
            IRoleRepository roleRepo,
            ILogger<AccountService> logger)
        {
            _jwtHelper = jwtHelper;
            _userRepo = userRepo;
            _tokenRepo = tokenRepo;
            _roleRepo = roleRepo;
            _logger = logger;
        }

        public BaseResponse AddRole(string role)
        {
            try
            {
                _logger.LogInformation($"[AddRole] Start to add role {role}");

                var ifRoleExist = _roleRepo.GetFirstOrNullAsync(t => t.RoleName == role);

                if (ifRoleExist != null)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, "Role has existed");
                }

                Role newRole = new Role
                {
                    RoleName = role,
                };

                _roleRepo.AddAsync(newRole);

                return new BaseResponse("Add role successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AddRole] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to add role");
            }
        }

        public BaseResponse AssignRole(string email, string role)
        {
            try
            {
                _logger.LogInformation($"[AssignRole] Start to assign role {role} to email {email}");

                var user = _userRepo.GetFirstOrNullAsync(t => !t.IsDeleted && t.UserName == email);

                if (user == null)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, "Username not found");
                }

                var roleName = _roleRepo.GetFirstAsync(t => t.RoleName == role);

                user.RoleId = roleName.RoleId;

                _userRepo.UpdateAsync(user);

                return new BaseResponse("Assign role successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Assign] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to assign role");
            }
        }

        public BaseResponse Login(LoginUser login)
        {
            try
            {
                _logger.LogInformation($"[Login] Start to login with email: {login.Email}");

                var user = _userRepo.GetFirstOrNullAsync(t => !t.IsDeleted && t.UserName == login.Email);

                if (user == null)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, "User not found");
                }

                var passwordValid = BCrypt.Net.BCrypt.EnhancedVerify(login.Password, user.Password);

                if (!passwordValid)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, "Password not correct");
                }

                var userRoles = _roleRepo.GetFirstOrNullAsync(t => t.RoleId == user.RoleId);

                var loginResponse = new LoginResponseModel
                {
                    Token = _jwtHelper.GenerateToken(user, userRoles.RoleName),
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Role = userRoles.RoleName ?? "",
                };

                var refreshToken = new Token
                {
                    RefreshToken = _jwtHelper.GenerateRefreshToken(),
                    ExpiredDate = DateTime.Now.AddDays(7),
                };

                _tokenRepo.AddAsync(refreshToken);

                loginResponse.RefreshToken = refreshToken.RefreshToken;
                
                return new BaseResponse(loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Login] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to login user");
            }
        }

        public BaseResponse RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                _logger.LogInformation($"[RefreshToken] Start to refresh token");

                var principle = _jwtHelper.GetTokenPrinciple(request.AccessToken);

                var user = _userRepo.GetFirstOrNullAsync(c => c.FullName == principle.Identity.Name);

                if (user == null)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, "Not found user with that token");
                }

                var isRefreshTokenValid = _tokenRepo.GetFirstOrNullAsync(c => c.RefreshToken == request.RefreshToken);

                if (isRefreshTokenValid == null)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, "Not found refresh token, please login");
                }

                if (isRefreshTokenValid.ExpiredDate <= DateTime.Now)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, "RefreshToken is not valid, please login again");
                }

                var response = new RefreshTokenResponse
                {
                    NewAccessToken = _jwtHelper.GenerateToken(user, request.RoleName)
                };

                return new BaseResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[RefreshToken] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to refresh token");
            }
        }

        public BaseResponse Register(RegisterUser register)
        {
            try
            {
                _logger.LogInformation($"[Register] Start to register user");

                var checkMail = _userRepo.GetFirstOrNullAsync(t => !t.IsDeleted && t.UserName == register.UserName);

                if (checkMail != null)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, "Email Exists!");
                }

                var role = _roleRepo.GetFirstOrNullAsync(t => t.RoleName == register.Role);

                if (role == null)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, "Role not found!");
                }

                var user = new User
                {
                    RoleId = role.RoleId,
                    UserName = register.UserName,
                    FullName = register.FullName,
                    Address = register.Address,
                    SignUpDate = DateOnly.FromDateTime(DateTime.Now),
                    IsDeleted = false,
                };

                user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(register.Password);

                _userRepo.AddAsync(user);

                return new BaseResponse("Register user successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Register] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to register user");
            }
        }

        public BaseResponse Logout(string refreshToken)
        {
            try
            {
                _logger.LogInformation($"[Logout] Start to logout");

                var isRefreshTokenValid = _tokenRepo.GetFirstOrNullAsync(c => c.RefreshToken == refreshToken);

                if (isRefreshTokenValid == null)
                {
                    return new BaseResponse(StatusCodes.Status400BadRequest, "User has already logged out");
                }

                _tokenRepo.DeleteAsync(isRefreshTokenValid);

                return new BaseResponse("Logout successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Logout] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to logout");
            }
        }
    }
}
