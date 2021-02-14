using MediatR;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Events;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Persistence.SqlLite
{
    public class PaymentRepository : IRequestHandler<SavePayment, Payment>, IRequestHandler<FindPayment, Payment>
    {
        private static readonly Regex CardNumber = new Regex(@"(\d{6})(\d{6})(\d{4})");

        public async Task<Payment> Handle(SavePayment request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var db = new PaymentGatewayDbContext())
            {
                // Implementing masking as suggested here: https://security.stackexchange.com/a/145079
                // CC numbers in the PaymentRequest are sanitised and should be just the numbers (no spaces)
                var maskedCardNumber = CardNumber.Replace(request.Request.CardNumber, @"$1******$3");

                var payment = new Payment(0,
                                          request.Request.MerchantId,
                                          request.Request.CardHolderName,
                                          maskedCardNumber,
                                          request.Request.ExpiryDate,
                                          request.Request.Money,
                                          request.Response.AcquiringBankPaymentId,
                                          request.Response.Status,
                                          request.Response.Timestamp);

                await db.AddAsync(payment);
                await db.SaveChangesAsync();

                return payment;
            }
        }

        public Task<Payment> Handle(FindPayment request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var db = new PaymentGatewayDbContext())
            {
                var payment = db.Payments
                    .Where(p => p.MerchantId == request.MerchantId && p.AcquiringBankPaymentId == request.AcquiringPaymentBankId)
                    .FirstOrDefault();

                return Task.FromResult(payment);
            }
        }
    }
}
