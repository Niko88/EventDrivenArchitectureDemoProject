using MassTransit;

namespace Orders.Infrastructure.Entities
{
    public class Order : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CustomerCode { get; set; }
        public string Description { get; internal set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string State { get; set; }
        public string PaymentStatus { get; set; }
        public string Message { get; set; }
        public int? PaymentId { get; set; }
        public Guid? TransactionId { get; set; }
        public string RevenueChannel { get; set; }
        public string RevenueCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string RedirectUrl { get; set; }
        public Uri ResponseAddress { get; set; }
        public Guid? RequestId { get; set; }
    }
}
