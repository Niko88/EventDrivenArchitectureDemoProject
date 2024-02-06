using System.Text.Json;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orders.Contracts.Commands;
using Orders.Contracts.Models;
using Orders.Contracts.PublishedEvents;
using Orders.Infrastructure.Repositories;

namespace Orders.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController(IRequestClient<InitiateOrder> initiateOrderRequestClient, IOrderRepository orderRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetOrder(Guid orderId)
        {
            var order= await orderRepository.GetOrder(orderId);

            return order is not null ?
                Ok(new
                {
                    OrderedItem = order.ItemCode,
                    OrderStatus = order.State,
                    PaymentStatus = order.PaymentStatus
                }) : 
                NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderDetails details)
        {
            var orderGenerationResponse= await initiateOrderRequestClient.GetResponse<OrderGenerationResult>(new InitiateOrder(
                Guid.NewGuid(),
                details)
            );

            return orderGenerationResponse.Message.IsSuccess ?
                Ok(new { RedirectUrl = orderGenerationResponse.Message.SessionUrl }) : 
                Problem(JsonSerializer.Serialize(new { Errors = orderGenerationResponse.Message.ErrorMessages }));
        }
    }
}
