using Payments.Contracts.Models;

namespace Payments.Application.Repositories;

public interface IPaymentsRepository
{
    Task<(bool IsSuccess, int Id)> StorePaymentDetailsAsync(PaymentDetails paymentDetails, string customerCode);
    Task<PaymentDetails?> UpdatePaymentStatus(PaymentNotification notification);
}