using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Payments.Contracts.Commands;
using Payments.Contracts.Models;

namespace Payments.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController(IPublishEndpoint messageBus) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Notify([FromBody] PaymentNotification notification)
        {
            await messageBus.Publish(new UpdatePaymentStatusCommand(notification));
            return Ok();
        }
    }
}
