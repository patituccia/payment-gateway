using MediatR;
using System;

namespace PaymentGateway.Domain.Events
{
    /// <summary>
    /// Domain event that is fired when a payment have been processed.
    /// </summary>
    public class PaymentProcessed : INotification
    {
        public PaymentProcessed(
            Payment payment,
            double findMerchantTime,
            double acquiringBankTime,
            double saveTime,
            double totalProcessingTime)
        {
            this.Payment = payment;
            this.FindMerchantTime = findMerchantTime;
            this.AcquiringBankTime = acquiringBankTime;
            this.SaveTime = saveTime;
            this.TotalProcessingTime = totalProcessingTime;
        }

        public Payment Payment { get; }

        public double FindMerchantTime { get; }

        public double AcquiringBankTime { get; }

        public double SaveTime { get; }

        public double TotalProcessingTime { get; }
    }
}
