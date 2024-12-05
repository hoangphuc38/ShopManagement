using ShopManagement_Backend_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, string role);

        string GenerateRefreshToken();

        ClaimsPrincipal? GetTokenPrinciple(string token);
    }
}
