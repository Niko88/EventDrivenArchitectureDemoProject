using Payments.Contracts.Commands;
using Payments.Contracts.Models;

namespace Payments.Application.Services;

public interface IPaymentProviderService
{
    public Task<PaymentSessionResult> CreateSession(GeneratePaymentSessionCommand  command, int paymentId);
}