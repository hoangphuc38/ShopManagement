using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopManagement_Backend_Application.Helpers;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Services;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ShopManagement_Backend_API.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly ILogger<AuthorizationMiddleware> _logger;
        private readonly IConfiguration _config;
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(
            ILogger<AuthorizationMiddleware> logger,
            IConfiguration config,
            RequestDelegate next)
        {
            _logger = logger;
            _config = config;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository userRepo, IRoleRepository roleRepo)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out _))
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                
                var validateToken = await ValidateToken(token);

                if (validateToken == null)
                {
                    await HandleExceptionAsync(context);
                    return;  
                }

                var userName = validateToken.Claims.First().Value;

                var userInfo = await userRepo.GetFirstOrNullAsync(c => c.UserName == userName);

                if (userInfo != null)
                {
                    var role = await roleRepo.GetFirstAsync(c => c.RoleId == userInfo.RoleId);

                    userInfo.Role = role;

                    context.Items["User"] = userInfo;
                }
            }

            await _next(context);
        }

        private async Task<JwtSecurityToken> ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JWTConfiguration:SecretKey"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken) validatedToken;
        }

        private async Task HandleExceptionAsync(HttpContext context)
        {
            BaseResponse response = new BaseResponse(
                        StatusCodes.Status401Unauthorized, "Unauthorized");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.Status;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
