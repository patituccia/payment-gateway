using System;

namespace PaymentGateway.Domain
{
    public class Payment
    {
        public Payment(int id, string maskedCardNumber, DateTime expiryDate, Money money, string acquiringBankIdentifier, PaymentStatus status)
        {
            if (string.IsNullOrEmpty(maskedCardNumber))
            {
                throw new ArgumentException($"'{nameof(maskedCardNumber)}' cannot be null or empty", nameof(maskedCardNumber));
            }

            if (string.IsNullOrEmpty(acquiringBankIdentifier))
            {
                throw new ArgumentException($"'{nameof(acquiringBankIdentifier)}' cannot be null or empty", nameof(acquiringBankIdentifier));
            }

            this.Id = id;
            this.MaskedCardNumber = maskedCardNumber;
            this.ExpiryDate = expiryDate;
            this.Money = money ?? throw new ArgumentNullException(nameof(money));
            this.AcquiringBankIdentifier = acquiringBankIdentifier;
            this.Status = status;
        }

        public int Id { get; }

        public string MaskedCardNumber { get; }

        public DateTime ExpiryDate { get; }

        public Money Money { get; }

        public string AcquiringBankIdentifier { get; }

        public PaymentStatus Status { get; }
    }
}
