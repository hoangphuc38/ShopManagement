using Microsoft.AspNetCore.Mvc;
using ShopManagement_Backend_API.CustomAttributes;
using ShopManagement_Backend_Application.Models.Token;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        [AllowAnonymousAttribute]
        public async Task<IActionResult> Register([FromBody] RegisterUser register)
        {
            var result = await _accountService.Register(register);

            return StatusCode(result.Status, result);
        }

        [HttpPost("login")]
        [AllowAnonymousAttribute]
        public async Task<IActionResult> Login([FromBody] LoginUser login)
        {
            var result = await _accountService.Login(login);

            return StatusCode(result.Status, result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _accountService.RefreshToken(request);

            return StatusCode(result.Status, result);
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(string role)
        {
            var result = await _accountService.AddRole(role);

            return StatusCode(result.Status, result);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(string email, string role)
        {
            var result = await _accountService.AssignRole(email, role);

            return StatusCode(result.Status, result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(int userID)
        {
            var result = await _accountService.Logout(userID);

            return StatusCode(result.Status, result);
        }
    }
}
