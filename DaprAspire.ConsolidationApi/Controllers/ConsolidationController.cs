using Microsoft.AspNetCore.Mvc;
using Dapr;
using DaprAspire.Domain.CrossCutting.Messaging.Events;

namespace DaprAspire.ConsolidationApi.Controllers
{
    [ApiController]
    public class ConsolidationController : ControllerBase
    {
        [Topic("pubsub", "entries.events")]
        [HttpPost("/events/entries")]
        public Task Handle(EntryEventMessage payload)
        {
            Console.WriteLine($"Evento recebido: {payload}");
            return Task.CompletedTask;
        }
    }
}
