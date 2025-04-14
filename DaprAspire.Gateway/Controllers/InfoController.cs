using Microsoft.AspNetCore.Mvc;

namespace DaprAspire.Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Gateway is running.");
    }
}
