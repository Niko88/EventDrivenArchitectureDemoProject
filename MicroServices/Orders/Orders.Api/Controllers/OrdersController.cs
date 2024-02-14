using System.Text.Json;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orders.Contracts.Commands;
using Orders.Contracts.Models;
using Orders.Contracts.PublishedEvents;
using Orders.Contracts.Queries;

namespace Orders.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController(
        IRequestClient<InitiateOrderCommand> initiateOrderRequestClient, 
        IRequestClient<GetOrderStatusQuery> getOrderDetailsRequestClient
    ) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetOrder(Guid orderId)
        {
            var orderStatusResponse= await getOrderDetailsRequestClient.GetResponse<OrderStatus>(new GetOrderStatusQuery(orderId));

            return Ok(orderStatusResponse.Message);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderDetails details)
        {
            var orderGenerationResponse= await initiateOrderRequestClient.GetResponse<OrderGenerationResult>(new InitiateOrderCommand(
                Guid.NewGuid(),
                details)
            );

            return orderGenerationResponse.Message.IsSuccess ?
                Ok(new { RedirectUrl = orderGenerationResponse.Message.SessionUrl }) : 
                Problem(JsonSerializer.Serialize(new { Errors = orderGenerationResponse.Message.ErrorMessages }));
        }
    }
}
