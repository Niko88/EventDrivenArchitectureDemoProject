namespace Payments.Infrastructure.SquirrelPay.Contracts;

public record GenerateSessionRequestModel(
    int PaymentId,
    decimal Amount,
    string NotifyUrl,
    string RedirectUrl
);