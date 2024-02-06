namespace PaymentProvider.Models;

public class SessionDetailsModel
{
    public int PaymentId { get; set; }
    public decimal Amount{ get; set; }
    public string NotifyUrl { get; set; }
    public string RedirectUrl { get; set; }
}