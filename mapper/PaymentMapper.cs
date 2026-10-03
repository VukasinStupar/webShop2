using webShop2.dto.payment;
using webShop2.model;

namespace webShop2.mapper;

public class PaymentMapper
{
    public PaymentDto ToDto(Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            Provider = payment.Provider,
            Status = payment.Status,
            TransactionId = payment.TransactionId,
            PaidAt = payment.PaidAt,
            FailureReason = payment.FailureReason
        };
    }

    public Payment ToEntity(CreatePaymentDto dto)
    {
        return new Payment
        {
            OrderId = dto.OrderId,
            Provider = dto.Provider
        };
    }

    public void UpdateEntity(Payment payment, UpdatePaymentDto dto)
    {
        payment.Status = dto.Status;
        payment.TransactionId = dto.TransactionId;
        payment.PaidAt = dto.PaidAt;
        payment.FailureReason = dto.FailureReason;
    }
}
