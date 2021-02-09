using MediatR;
using PaymentGateway.Domain.Events;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    /// <summary>
    /// Represents a payment request.
    /// </summary>
    public class PaymentRequest
    {
        private readonly IAcquiringBank acquiringBank;
        private readonly IMediator mediator;
        private bool isProcessed = false;

        internal PaymentRequest(IAcquiringBank acquiringBank, IMediator mediator, string cardNumber, DateTime expiryDate, Money money, string cVV)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                throw new ArgumentException($"'{nameof(cardNumber)}' cannot be null or empty", nameof(cardNumber));
            }

            this.CardNumber = cardNumber;
            this.ExpiryDate = expiryDate;
            this.Money = money ?? throw new ArgumentNullException(nameof(money));
            this.CVV = cVV;
            this.acquiringBank = acquiringBank ?? throw new ArgumentNullException(nameof(acquiringBank));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public string CardNumber { get; }

        public DateTime ExpiryDate { get; }

        public Money Money { get; }

        public string CVV { get; }

        public async Task<Payment> Process()
        {
            if (this.isProcessed)
            {
                throw new InvalidOperationException("Request have been processed already.");
            }

            var response = await this.acquiringBank.Process(this);
            var payment = await this.mediator.Send(new SavePayment(this, response));

            this.isProcessed = true;

            return payment;
        }
    }
}
