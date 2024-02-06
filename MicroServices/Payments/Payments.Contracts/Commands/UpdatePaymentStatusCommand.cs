using Payments.Contracts.Models;

namespace Payments.Contracts.Commands;

public record UpdatePaymentStatusCommand(PaymentNotification NotificationContent);
