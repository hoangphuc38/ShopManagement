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
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ShopManagement_Backend_Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserRepository _userRepo;
        private readonly ITokenRepository _tokenRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly ILogger<AccountService> _logger;
        private readonly ResourceManager _resource;

        public AccountService(
            IJwtHelper jwtHelper,
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
            _resource = new ResourceManager(
                "ShopManagement_Backend_Application.Resources.Messages.AccountMessages",
                Assembly.GetExecutingAssembly());
        }

        public async Task<BaseResponse> AddRole(string role)
        {
            try
            {
                _logger.LogInformation($"[AddRole] Start to add role {role}");

                var ifRoleExist = await _roleRepo.GetFirstOrNullAsync(t => t.RoleName == role);

                if (ifRoleExist != null)
                {
                    return new BaseResponse(
                        StatusCodes.Status400BadRequest, 
                        _resource.GetString("RoleExist") ?? "");
                }

                Role newRole = new Role
                {
                    RoleName = role,
                };

                await _roleRepo.AddAsync(newRole);

                return new BaseResponse(_resource.GetString("RoleSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[AddRole] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError, 
                    _resource.GetString("AddRoleFailed") ?? "");
            }
        }

        public async Task<BaseResponse> AssignRole(string email, string role)
        {
            try
            {
                _logger.LogInformation($"[AssignRole] Start to assign role {role} to email {email}");

                var user = await _userRepo.GetFirstOrNullAsync(t => !t.IsDeleted && t.UserName == email);

                if (user == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status400BadRequest, 
                        _resource.GetString("NotFoundUser") ?? "");
                }

                var roleName = await _roleRepo.GetFirstAsync(t => t.RoleName == role);

                user.RoleId = roleName.RoleId;

                await _userRepo.UpdateAsync(user);

                return new BaseResponse(_resource.GetString("AssignRoleSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Assign] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError, 
                    _resource.GetString("AssignRoleFailed") ?? "");
            }
        }

        public async Task<BaseResponse> Login(LoginUser login)
        {
            try
            {
                _logger.LogInformation($"[Login] Start to login with email: {login.Email}");

                var user = await _userRepo.GetFirstOrNullAsync(t => !t.IsDeleted && t.UserName == login.Email);

                if (user == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status400BadRequest,
                        _resource.GetString("NotFoundUser") ?? "");
                }

                var passwordValid = BCrypt.Net.BCrypt.EnhancedVerify(login.Password, user.Password);

                if (!passwordValid)
                {
                    return new BaseResponse(
                        StatusCodes.Status400BadRequest, 
                        _resource.GetString("InvalidPassword") ?? "");
                }

                var userRoles = await _roleRepo.GetFirstOrNullAsync(t => t.RoleId == user.RoleId);

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
                    UserID = user.Id,
                };

                await _tokenRepo.AddAsync(refreshToken);

                loginResponse.RefreshToken = refreshToken.RefreshToken;
                
                return new BaseResponse(loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Login] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("LoginFailed") ?? "");
            }
        }

        public async Task<BaseResponse> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                _logger.LogInformation($"[RefreshToken] Start to refresh token");

                var isRefreshTokenValid = await _tokenRepo.GetFirstOrNullAsync(c => c.RefreshToken == request.RefreshToken);

                if (isRefreshTokenValid == null || isRefreshTokenValid.ExpiredDate <= DateTime.Now)
                {
                    return new BaseResponse(
                        StatusCodes.Status400BadRequest,
                        _resource.GetString("NotFoundRefreshToken") ?? "");
                }

                var user = await _userRepo.GetFirstOrNullAsync(c => c.Id == isRefreshTokenValid.UserID);

                if (user == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status400BadRequest,
                        _resource.GetString("NotFoundUserWithToken") ?? "");
                }

                var role = await _roleRepo.GetFirstOrNullAsync(c => c.RoleId == user.RoleId);

                if (role == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status500InternalServerError,
                        _resource.GetString("NotFoundRole") ?? "");
                }

                var response = new RefreshTokenResponse
                {
                    NewAccessToken = _jwtHelper.GenerateToken(user, role.RoleName)
                };

                return new BaseResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[RefreshToken] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("RefreshTokenFailed") ?? "");
            }
        }

        public async Task<BaseResponse> Register(RegisterUser register)
        {
            try
            {
                _logger.LogInformation($"[Register] Start to register user");

                var checkMail = await _userRepo.GetFirstOrNullAsync(t => !t.IsDeleted && t.UserName == register.UserName);

                if (checkMail != null)
                {
                    return new BaseResponse(
                        StatusCodes.Status400BadRequest,
                        _resource.GetString("MailExist") ?? "");
                }

                var role = await _roleRepo.GetFirstOrNullAsync(t => t.RoleName == register.Role);

                if (role == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status400BadRequest,
                        _resource.GetString("RoleNotExist") ?? "");
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

                await _userRepo.AddAsync(user);

                return new BaseResponse(_resource.GetString("RegisterSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Register] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("RegisterFailed") ?? "");
            }
        }

        public async Task<BaseResponse> Logout(int userID)
        {
            try
            {
                _logger.LogInformation($"[Logout] Start to logout");

                var isRefreshTokenValid = await _tokenRepo.GetFirstOrNullAsync(c => c.UserID == userID);

                if (isRefreshTokenValid == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status400BadRequest,
                        _resource.GetString("AlreadyLogout") ?? "");
                }

                await _tokenRepo.DeleteAsync(isRefreshTokenValid);

                return new BaseResponse(_resource.GetString("LogoutSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Logout] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("LogoutFailed") ?? "");
            }
        }
    }
}
