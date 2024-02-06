using ExampleStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using ExampleStore.Services;

namespace ExampleStore.Controllers
{
    public class HomeController(OrdersApiService ordersApiService, StockApiService stockApiService) : Controller
    {
        public async Task<IActionResult> Pay(HomeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await ordersApiService.GetRedirect(viewModel);
                var redirect = JsonSerializer.Deserialize<SessionResponse>(response);
                return Redirect(redirect.RedirectUrl);
            }
            return View("Index",viewModel);
        }

        public async Task<IActionResult> Index()
        {
            var stock = await stockApiService.GetStockItems();
            var viewModel = new HomeViewModel
            {
                itemDetails = stock.StockItems.FirstOrDefault()
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
