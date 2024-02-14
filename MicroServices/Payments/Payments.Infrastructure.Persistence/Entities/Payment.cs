namespace Payments.Infrastructure.Entities;

public class Payment
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public string RelatedEntityId { get; set; }
    public string CustomerCode { get; set; }
    public decimal Amount { get; set; }
    public Guid? TransactionId { get; set; }
    public string SessionId { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }
}