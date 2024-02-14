using Microsoft.EntityFrameworkCore;
using Payments.Application.Repositories;
using Payments.Contracts.Models;
using Payments.Infrastructure.DbContexts;
using Payments.Infrastructure.Entities;

namespace Payments.Infrastructure.Repositories;

public class PaymentsRepository(PaymentsContext dbContext) : IPaymentsRepository
{
    public async Task<(bool IsSuccess, int Id)> StorePaymentDetailsAsync(PaymentDetails paymentDetails, string customerCode)
    {
        try
        {
            var newPayment = new Payment()
            {
                Amount = paymentDetails.Price,
                CreationDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                Status = "Created",
                Message = "Payment Created",
                RelatedEntityId = paymentDetails.OrderId,
                CustomerCode = customerCode,
                SessionId = "Not available",
            };

            var paymentSaved = await dbContext.Payments.AddAsync(newPayment);
            await dbContext.SaveChangesAsync();

            return (true, paymentSaved.Entity.Id);
        }
        catch
        {
            return (false, -1);
        }
    }

    public async Task<PaymentDetails?> UpdatePaymentStatus(PaymentNotification notification)
    {
        var paymentDetails = await dbContext.Payments.FirstOrDefaultAsync(p => p.Id == notification.PaymentId);

        try
        {
            paymentDetails.Status = notification.IsSuccess ? "success" : "fail";
            paymentDetails.TransactionId = notification.TransactionId;
            dbContext.Update(paymentDetails);
            await dbContext.SaveChangesAsync();
            return new PaymentDetails(
                OrderId: paymentDetails.RelatedEntityId,
                Price: paymentDetails.Amount,
                TransactionId: notification.TransactionId
            );
        }
        catch 
        {
            return null;
        }
    }
}