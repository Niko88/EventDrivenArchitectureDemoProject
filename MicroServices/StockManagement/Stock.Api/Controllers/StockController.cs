using Microsoft.AspNetCore.Mvc;
using Stock.Application.Services;

namespace Stock.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController(IStockRepository stockRepository) : ControllerBase
    {
        public async Task<ActionResult> Get() => Ok(new {
            StockItems = await stockRepository.GetAvailableStock()
        });
    }
}
