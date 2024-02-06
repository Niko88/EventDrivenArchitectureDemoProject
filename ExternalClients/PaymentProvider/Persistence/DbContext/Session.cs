namespace PaymentProvider.Persistence.DbContext;

public class Session
{
    public string Id { get; set; } 
    public int PaymentId { get; set; }
    public decimal Amount { get; set; }
    public bool IsSuccess { get; set; }
    public string NotifyUrl { get; set; }
    public string RedirectUrl { get; set; }
    public Guid? TransactionId { get; set; }
}