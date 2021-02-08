using MediatR;
using System;
using System.Text.RegularExpressions;

namespace PaymentGateway.Domain
{
    /// <summary>
    /// Factory for <see cref="PaymentRequest"/> instances to inject required dependencies.
    /// </summary>
    public class PaymentRequestFactory : IPaymentRequestFactory
    {
        private static Regex CardNumber = new Regex(@"\d{16}");

        private readonly IAcquiringBank acquiringBank;
        private readonly IMediator mediator;

        public PaymentRequestFactory(IAcquiringBank acquiringBank, IMediator mediator)
        {
            this.acquiringBank = acquiringBank;
            this.mediator = mediator;
        }

        public PaymentRequest Create(string cardNumber, DateTime expiryDate, Money money, int cVV)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                throw new ArgumentException($"'{nameof(cardNumber)}' cannot be null or empty", nameof(cardNumber));
            }

            if (money is null)
            {
                throw new ArgumentNullException(nameof(money));
            }

            if (!CardNumber.IsMatch(cardNumber))
            {
                throw new ArgumentException("Card number is invalid.", "cardNumber");
            }

            return new PaymentRequest(this.acquiringBank, this.mediator, cardNumber, expiryDate, money, cVV);
        }
    }
}
