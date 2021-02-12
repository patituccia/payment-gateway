using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    /// <summary>
    /// Represents a payment request to be issued to an aquiring bank on behalf of a merchant.
    /// </summary>
    public class PaymentRequestDto
    {
        /// <summary>
        /// The payment gateway's merchant id.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int MerchantId { get; set; }

        /// <summary>
        /// The card holder name.
        /// </summary>
        [Required]
        public string CardHolderName { get; set; }

        /// <summary>
        /// A valid credit card number.
        /// </summary>
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        /// <summary>
        /// The credit card expiry date.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// The payment amount.
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        /// <summary>
        /// The ISO 4217 currency code.
        /// </summary>
        [Required]
        [RegularExpression(@"^[A-Z]{3}$")]
        public string Currency { get; set; }

        /// <summary>
        /// The credit card CVV number.
        /// </summary>
        [Required]
        [RegularExpression(@"^[0-9]{3,4}$")]
        public string CVV { get; set;  }
    }
}
