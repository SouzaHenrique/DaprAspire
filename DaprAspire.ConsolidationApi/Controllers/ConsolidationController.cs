using Microsoft.AspNetCore.Mvc;
using Dapr;

namespace DaprAspire.ConsolidationApi.Controllers
{
    [ApiController]
    public class ConsolidationController : ControllerBase
    {
        [Topic("pubsub", "entries.events")]
        [HttpPost("/events/entries")]
        public Task Handle(dynamic payload)
        {
            Console.WriteLine($"Evento recebido: {payload}");
            return Task.CompletedTask;
        }
    }
}
