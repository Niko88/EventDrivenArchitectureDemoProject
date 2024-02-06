using ExampleStore.Models;

namespace ExampleStore.Services;

public class StockApiService(IHttpClientFactory clientFactory)
{
    public async Task<StockResponse?> GetStockItems()
    {
        using var httpClient = clientFactory.CreateClient("StockApi");

        return await httpClient.GetFromJsonAsync<StockResponse>("");
    }

}