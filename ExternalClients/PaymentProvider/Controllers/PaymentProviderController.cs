using Microsoft.AspNetCore.Mvc;
using PaymentProvider.Models;
using PaymentProvider.Persistence.DbContext;
using System.Diagnostics;

namespace PaymentProvider.Controllers
{
    public class PaymentProviderController(PaymentSessionContext dbContext) : Controller
    {

        [HttpPost]
        public async Task<ActionResult> CreateSession([FromBody] SessionDetailsModel details)
        {
            var session = new Session
            {
                Id = Guid.NewGuid().ToString(), 
                PaymentId = details.PaymentId,
                RedirectUrl = details.RedirectUrl,
                NotifyUrl = details.NotifyUrl,
                Amount = details.Amount,
                IsSuccess = false
            };

            var addResult = await dbContext.Sessions.AddAsync(session);
            await dbContext.SaveChangesAsync();

            return Ok(new{ SessionUrl = $"https://localhost:7290/PaymentProvider/Pay?sessionId={addResult.Entity.Id}" });
        }


        public IActionResult Pay(string sessionId)
        {
            var session = dbContext.Sessions.FirstOrDefault(s => s.Id == sessionId);
            if (session is null)
                return RedirectToAction("Error");
            return View(new PayViewModel(sessionId, session.Amount));
        }

        public async Task<IActionResult> PayResult(PayViewModel viewModel)
        {
            var session = dbContext.Sessions.FirstOrDefault(s => s.Id == viewModel.SessionId);
            session.IsSuccess = viewModel.IsSuccess;
            session.TransactionId = Guid.NewGuid();
            dbContext.Update(session);
            dbContext.SaveChangesAsync();
            using (var client = new HttpClient())
            {
                await client.PostAsJsonAsync(session.NotifyUrl, new
                {
                    session.PaymentId,
                    session.IsSuccess,
                    session.TransactionId
                });
            }

            return View(new PayResultViewModel()
            {
                PaymentIsSuccessful = session.IsSuccess,
                RedirectUrl = session.RedirectUrl
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
