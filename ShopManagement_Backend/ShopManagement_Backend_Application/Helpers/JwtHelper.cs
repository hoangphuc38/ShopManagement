using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ShopManagement_Backend_Application.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user, string role)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.UserName)
            };

            if (!string.IsNullOrEmpty(role))
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWTConfiguration:SecretKey"])
                );

            var token = new JwtSecurityToken(
                //issuer: _config["JWT:ValidIssuer"],
                //audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    authenKey,
                    SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
