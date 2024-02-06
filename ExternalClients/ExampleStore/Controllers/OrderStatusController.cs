using ExampleStore.Models;
using ExampleStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExampleStore.Controllers
{
    public class OrderStatusController(OrdersApiService ordersApiService) : Controller
    {
        public async Task<IActionResult> Index(Guid orderId)
        {
            var status = await ordersApiService.GetOrder(orderId);
            return View(new OrderStatusViewModel
            {
                OrderFound = status is not null,
                ResponseContent = status
            });
        }
    }
}
