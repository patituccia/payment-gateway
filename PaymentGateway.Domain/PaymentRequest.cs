using MediatR;
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

        public PaymentRequest(string cardNumber, DateTime expiryDate, Money money, string cVV)
        {
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
                throw new ArgumentException("Card number is invalid.", "cardNumber");
            }

            this.CardNumber = sanitisedCardNumber;
            this.ExpiryDate = expiryDate;
            this.Money = money ?? throw new ArgumentNullException(nameof(money));
            this.CVV = cVV;
        }

        public string CardNumber { get; }

        public DateTime ExpiryDate { get; }

        public Money Money { get; }

        public string CVV { get; }

        public bool IsProcessed { get; internal set; } = false;
    }
}
