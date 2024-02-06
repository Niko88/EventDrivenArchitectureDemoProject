
using Payments.Contracts.Models;

namespace Payments.Contracts.Commands
{
    public record GeneratePaymentSessionCommand(
        Guid CorrelationId, 
        string CustomerCode, 
        PaymentDetails PaymentDetails, 
        string Description,
        string RedirectUrl
     );
}
