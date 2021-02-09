using System;
using System.Text.RegularExpressions;

namespace PaymentGateway.Domain
{
    /// <summary>
    /// Represents a decimal amount in a specified currency.
    /// </summary>
    /// <remarks>
    /// Based on the pattern descibed here: https://martinfowler.com/eaaCatalog/money.html+
    /// </remarks>
    public class Money
    {
        private static Regex currencyRegex = new Regex(@"^[A-Z]{3}$");

        public Money(decimal amount, string currency)
        {
            if (currency == null)
            {
                throw new System.ArgumentNullException(nameof(currency));
            }

            // Here we are doing some basic validation of the currency code - should be properly done against https://en.wikipedia.org/wiki/ISO_4217
            if (!currencyRegex.IsMatch(currency))
            {
                throw new ArgumentException("Invalid currency code", "currency");
            }

            this.Amount = amount;
            this.Currency = currency;
        }

        /// <summary>
        /// The amount represented in this instance.
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Represents a ISO 4217 currency code.
        /// To be replaced by a proper Currency class which supports validation of the codes.
        /// </summary>
        public string Currency { get; private set; }
    }
}
