namespace PaymentProvider.Models;

public class PayViewModel
{
    public PayViewModel(string sessionId, decimal amount)
    {
        SessionId = sessionId;
        Amount = amount;
    }

    public PayViewModel()
    {
        
    }
    public string SessionId { get; set; }
    public bool IsSuccess { get; set; }
    public decimal Amount { get; set;  }
}