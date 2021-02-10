using MediatR;
using PaymentGateway.Domain.Events;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    /// <summary>
    /// Factory for <see cref="PaymentRequest"/> instances to inject required dependencies.
    /// </summary>
    public class PaymentRequestProcessor : IPaymentRequestProcessor
    {
        private static Regex CardNumber = new Regex(@"\d{16}");

        private readonly IAcquiringBank acquiringBank;
        private readonly IMediator mediator;

        public PaymentRequestProcessor(IAcquiringBank acquiringBank, IMediator mediator)
        {
            this.acquiringBank = acquiringBank;
            this.mediator = mediator;
        }

        public async Task<Payment> Process(PaymentRequest request)
        {
            if (request.IsProcessed)
            {
                throw new InvalidOperationException("Request have been processed already.");
            }

            var merchant = await this.mediator.Send(new FindMerchant(request.MerchantId));
            if (merchant == null)
            {
                throw new InvalidOperationException($"Merchant with Id: {request.MerchantId} not found.");
            }

            var response = await this.acquiringBank.Process(request);
            var payment = await this.mediator.Send(new SavePayment(request, response));

            request.IsProcessed = true;

            return payment;
        }
    }
}
