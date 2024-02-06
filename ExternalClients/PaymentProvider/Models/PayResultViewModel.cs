namespace PaymentProvider.Models;

public class PayResultViewModel
{
    public bool PaymentIsSuccessful { get; set; }
    public string RedirectUrl{ get; set; }
}