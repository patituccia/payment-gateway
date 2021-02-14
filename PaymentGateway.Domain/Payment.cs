using System;

namespace PaymentGateway.Domain
{
    public class Payment
    {
        private Payment()
        {
        }

        public Payment(int id,
                       int merchantId,
                       string cardHolderName,
                       string maskedCardNumber,
                       DateTime expiryDate,
                       Money money,
                       string acquiringBankPaymentId,
                       PaymentStatus status,
                       DateTime timestamp)
        {
            if (string.IsNullOrEmpty(cardHolderName))
            {
                throw new ArgumentException($"'{nameof(cardHolderName)}' cannot be null or empty", nameof(cardHolderName));
            }

            if (string.IsNullOrEmpty(maskedCardNumber))
            {
                throw new ArgumentException($"'{nameof(maskedCardNumber)}' cannot be null or empty", nameof(maskedCardNumber));
            }

            if (string.IsNullOrEmpty(acquiringBankPaymentId))
            {
                throw new ArgumentException($"'{nameof(acquiringBankPaymentId)}' cannot be null or empty", nameof(acquiringBankPaymentId));
            }

            this.Id = id;
            this.MerchantId = merchantId;
            this.CardHolderName = cardHolderName;
            this.MaskedCardNumber = maskedCardNumber;
            this.ExpiryDate = expiryDate;
            this.Money = money ?? throw new ArgumentNullException(nameof(money));
            this.AcquiringBankPaymentId = acquiringBankPaymentId;
            this.Status = status;
            this.Timestamp = timestamp;
        }

        public int Id { get; private set; }

        public int MerchantId { get; private set; }

        public string CardHolderName { get; private set; }

        public string MaskedCardNumber { get; private set; }

        public DateTime ExpiryDate { get; private set; }

        public Money Money { get; private set; }

        public string AcquiringBankPaymentId { get; private set; }

        public PaymentStatus Status { get; private set; }

        public DateTime Timestamp { get; private set; }
    }
}
