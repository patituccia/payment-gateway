using MediatR;
using PaymentGateway.Domain.Events;
using System;
using System.Diagnostics;
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
            var stopwatch = Stopwatch.StartNew();

            if (request.IsProcessed)
            {
                throw new InvalidOperationException("Request have been processed already.");
            }

            var merchant = await this.mediator.Send(new FindMerchant(request.MerchantId));

            var findMerchantTime = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Restart();

            if (merchant == null)
            {
                throw new InvalidOperationException($"Merchant with Id: {request.MerchantId} not found.");
            }

            var response = await this.acquiringBank.Process(request);

            var acquiringBankTime = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Restart();

            var payment = await this.mediator.Send(new SavePayment(request, response));

            var saveTime = stopwatch.Elapsed.TotalMilliseconds;

            // This await is just for the publish operation not the actual handling operation.
            await this.mediator.Publish(new PaymentProcessed(payment, findMerchantTime, acquiringBankTime, saveTime, findMerchantTime + acquiringBankTime + saveTime));

            request.IsProcessed = true;

            return payment;
        }
    }
}
