using DaprAspire.Domain.CrossCutting.DTO;
using DaprAspire.IdentityService.Application.Services.Auth;

using Microsoft.AspNetCore.Mvc;

namespace DaprAspire.IdentityService.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(AuthService authService, ILogger<AccountController> logger) : ControllerBase
    {
        private readonly ILogger<AccountController> _logger = logger;
        private readonly AuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Tentativa de login para o usuário '{Username}'", request.Username);

            var (success, token, error) = await _authService.LoginAsync(request.Username, request.Password);

            if (!success)
            {
                _logger.LogWarning("Falha de login para o usuário '{Username}'", request.Username);
                return Unauthorized(new { Message = "Usuário ou senha inválidos." });
            }

            _logger.LogInformation("Login bem-sucedido para o usuário '{Username}'", request.Username);
            return Ok(new { access_token = token });
        }
    }
}


