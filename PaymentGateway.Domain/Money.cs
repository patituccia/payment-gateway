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
        public Money(decimal amount, string currency)
        {
            if (currency == null)
            {
                throw new System.ArgumentNullException(nameof(currency));
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
