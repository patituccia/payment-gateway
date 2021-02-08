using System;

namespace PaymentGateway.Domain
{
    public interface IPaymentRequestFactory
    {
        PaymentRequest Create(string cardNumber, DateTime expiryDate, Money money, int cVV);
    }
}
