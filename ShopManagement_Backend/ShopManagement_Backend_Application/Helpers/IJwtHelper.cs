using ShopManagement_Backend_Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShopManagement_Backend_Application.Helpers
{
    public interface IJwtHelper
    {
        string GenerateToken(User user, string role);

        string GenerateRefreshToken();

        ClaimsPrincipal? GetTokenPrinciple(string token);
    }
}
