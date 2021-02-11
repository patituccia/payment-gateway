using System;

namespace PaymentGateway.Domain
{
    public class Payment
    {
        public Payment(int id,
                       string maskedCardNumber,
                       DateTime expiryDate,
                       Money money,
                       string acquiringBankPaymentId,
                       PaymentStatus status,
                       DateTime timestamp)
        {
            if (string.IsNullOrEmpty(maskedCardNumber))
            {
                throw new ArgumentException($"'{nameof(maskedCardNumber)}' cannot be null or empty", nameof(maskedCardNumber));
            }

            if (string.IsNullOrEmpty(acquiringBankPaymentId))
            {
                throw new ArgumentException($"'{nameof(acquiringBankPaymentId)}' cannot be null or empty", nameof(acquiringBankPaymentId));
            }

            this.Id = id;
            this.MaskedCardNumber = maskedCardNumber;
            this.ExpiryDate = expiryDate;
            this.Money = money ?? throw new ArgumentNullException(nameof(money));
            this.AcquiringBankPaymentId = acquiringBankPaymentId;
            this.Status = status;
            this.Timestamp = timestamp;
        }

        public int Id { get; }

        public string MaskedCardNumber { get; }

        public DateTime ExpiryDate { get; }

        public Money Money { get; }

        public string AcquiringBankPaymentId { get; }

        public PaymentStatus Status { get; }

        public DateTime Timestamp { get; }
    }
}
