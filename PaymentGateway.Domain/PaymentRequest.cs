using System;
using System.Text.RegularExpressions;

namespace PaymentGateway.Domain
{
    /// <summary>
    /// Represents a payment request.
    /// </summary>
    public class PaymentRequest
    {
        private static Regex CardNumberRegex = new Regex(@"\d{16}");

        private static Regex CvvRegex = new Regex(@"^[0-9]{3,4}$");

        public PaymentRequest(int merchantId, string cardHolderName, string cardNumber, DateTime expiryDate, Money money, string cVV)
        {
            if (string.IsNullOrEmpty(cardHolderName))
            {
                throw new ArgumentException($"'{nameof(cardHolderName)}' cannot be null or empty", nameof(cardHolderName));
            }

            if (string.IsNullOrEmpty(cardNumber))
            {
                throw new ArgumentException($"'{nameof(cardNumber)}' cannot be null or empty", nameof(cardNumber));
            }

            if (money is null)
            {
                throw new ArgumentNullException(nameof(money));
            }

            // TODO: Here we perform just some basic validation on the CC number which should be replaced with a validation class/service.
            var sanitisedCardNumber = cardNumber.Replace(" ", string.Empty);
            if (!CardNumberRegex.IsMatch(sanitisedCardNumber))
            {
                throw new ArgumentException("Card number is invalid.", nameof(cardNumber));
            }

            if (!CvvRegex.IsMatch(cVV))
            {
                throw new ArgumentException("CVV is invalid.", nameof(cVV));
            }

            this.MerchantId = merchantId;
            this.CardHolderName = cardHolderName;
            this.CardNumber = sanitisedCardNumber;
            this.ExpiryDate = expiryDate;
            this.Money = money ?? throw new ArgumentNullException(nameof(money));
            this.CVV = cVV;
        }

        public int MerchantId { get; }

        public string CardHolderName { get; }

        public string CardNumber { get; }

        public DateTime ExpiryDate { get; }

        public Money Money { get; }

        public string CVV { get; }

        public bool IsProcessed { get; internal set; }
    }
}
