namespace Orders.Contracts.PublishedEvents
{
    public record OrderGenerationResult(
        Guid CorrelationId,
        bool IsSuccess,
        string? ErrorMessages = null,
        string SessionUrl = "Unavailable",
        int? PaymentId = null
    );
}
