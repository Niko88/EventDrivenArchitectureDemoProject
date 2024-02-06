namespace Payments.Contracts.Models;

public class PaymentNotification
{
    public int? PaymentId { get; set; }
    public bool IsSuccess{ get; set; }
    public Guid TransactionId { get; set; }
}