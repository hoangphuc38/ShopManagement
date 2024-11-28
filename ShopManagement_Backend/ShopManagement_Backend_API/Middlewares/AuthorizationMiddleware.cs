using ShopManagement_Backend_Application.Helpers;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Services;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_API.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly ILogger<AuthorizationMiddleware> _logger;
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(
            ILogger<AuthorizationMiddleware> logger,
            RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository userRepo, IJwtHelper jwtHelper)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out _))
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                var principle = jwtHelper.GetTokenPrinciple(token);

                var userInfo = await userRepo.GetFirstOrNullAsync(c => c.FullName == principle.Identity.Name);

                if (userInfo != null)
                {
                    context.Items["User"] = userInfo;
                }
            }
            

            await _next(context);
        }
    }
}
