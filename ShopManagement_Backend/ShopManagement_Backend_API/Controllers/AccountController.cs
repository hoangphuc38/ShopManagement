using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_Application.Models.Token;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUser register)
        {
            var result = _accountService.Register(register);

            return StatusCode(result.Status, result);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUser login)
        {
            var result = _accountService.Login(login);

            return StatusCode(result.Status, result);
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = _accountService.RefreshToken(request);

            return StatusCode(result.Status, result);
        }

        [HttpPost("add-role")]
        public IActionResult AddRole(string role)
        {
            var result = _accountService.AddRole(role);

            return StatusCode(result.Status, result);
        }

        [HttpPost("assign-role")]
        public IActionResult AssignRole(string email, string role)
        {
            var result = _accountService.AssignRole(email, role);

            return StatusCode(result.Status, result);
        }

        [HttpPost("logout")]
        public IActionResult Logout(string refreshToken)
        {
            var result = _accountService.Logout(refreshToken);

            return StatusCode(result.Status, result);
        }
    }
}
