using DaprAspire.IdentityService.Application.Services.Auth;

using Microsoft.AspNetCore.Mvc;

namespace DaprAspire.IdentityService.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(AuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (success, token, error) = await authService.LoginAsync(request.Username, request.Password);

            if (!success)
                return Unauthorized(new { message = error });

            return Ok(new { access_token = token });
        }
    }

    public record LoginRequest(string Username, string Password);
}


