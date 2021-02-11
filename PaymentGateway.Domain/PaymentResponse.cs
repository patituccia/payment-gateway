using System;

namespace PaymentGateway.Domain
{
    public class PaymentResponse
    {
        public PaymentResponse(string acquiringBankPaymentId, PaymentStatus status, DateTime timestamp)
        {
            if (string.IsNullOrEmpty(acquiringBankPaymentId))
            {
                throw new System.ArgumentException($"'{nameof(acquiringBankPaymentId)}' cannot be null or empty", nameof(acquiringBankPaymentId));
            }

            this.AcquiringBankPaymentId = acquiringBankPaymentId;
            this.Status = status;
            this.Timestamp = timestamp;
        }

        public string AcquiringBankPaymentId { get; }

        public PaymentStatus Status { get; }

        public DateTime Timestamp { get; }
    }
}
