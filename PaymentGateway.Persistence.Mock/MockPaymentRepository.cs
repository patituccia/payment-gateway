﻿using MediatR;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Events;
using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Persistence.Mock
{
    public class MockPaymentRepository : IRequestHandler<SavePayment, Payment>, IRequestHandler<FindPayment, Payment>
    {
        private static int NextPaymentId = 0;

        private static readonly ConcurrentDictionary<string, Payment> PaymentDb = new ConcurrentDictionary<string, Payment>();

        private static readonly Regex CardNumber = new Regex(@"(\d{6})(\d{6})(\d{4})");

        public Task<Payment> Handle(SavePayment request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Implementing masking as suggested here: https://security.stackexchange.com/a/145079
            // CC numbers in the PaymentRequest are sanitised and should be just the numbers (no spaces)
            var maskedCardNumber = CardNumber.Replace(request.Request.CardNumber, @"$1******$3");

            var payment = new Payment(this.GetNextPaymentId(),
                                      request.Request.MerchantId,
                                      request.Request.CardHolderName,
                                      maskedCardNumber,
                                      request.Request.ExpiryDate,
                                      request.Request.Money,
                                      request.Response.AcquiringBankPaymentId,
                                      request.Response.Status,
                                      request.Response.Timestamp);

            if (!PaymentDb.TryAdd(request.Response.AcquiringBankPaymentId, payment))
            {
                throw new InvalidOperationException($"The key {request.Response.AcquiringBankPaymentId} already exists in the dictionary");
            }

            return Task.FromResult(payment);
        }

        public Task<Payment> Handle(FindPayment request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (PaymentDb.TryGetValue(request.AcquiringPaymentBankId, out var payment))
            {
                return Task.FromResult(payment);
            }

            return Task.FromResult<Payment>(null);
        }

        private int GetNextPaymentId()
        {
            return Interlocked.Increment(ref NextPaymentId);
        }
    }
}
