using ExampleStore.Models;

namespace ExampleStore.Services
{
    public class OrdersApiService(IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        public async Task<string> GetRedirect(HomeViewModel viewModel)
        {
            using var httpClient = clientFactory.CreateClient("OrderApi");
            var response = await httpClient.PostAsJsonAsync("", new
            {
                ItemCode = viewModel.itemDetails.Sku,
                CustomerCode = viewModel.CustomerCode,
                Quantity = 1,
                Price = viewModel.itemDetails.Price,
                RedirectUrl = "https://localhost:5053/OrderStatus?orderId=",
                RevenueChannel = "ExampleShop",
                RevenueCode = viewModel.itemDetails.RevenueCode
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return $"Could not retrieve redirect sc: {response.StatusCode}";
        }
        public async Task<OrderStatusResponse?> GetOrder(Guid orderId)
        {
            using var httpClient = clientFactory.CreateClient("OrderApi");
            return await httpClient.GetFromJsonAsync<OrderStatusResponse>($"?orderId={orderId}");
        }
    }
}
