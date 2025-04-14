using Microsoft.AspNetCore.Mvc;

namespace DaprAspire.IdentityService.Api.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Identity Service is running.");
    }
}
