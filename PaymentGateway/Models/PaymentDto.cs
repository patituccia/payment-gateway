using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    /// <summary>
    /// Represents a payment processed by the gateway and stored for later retrieval.
    /// </summary>
    public class PaymentDto
    {
        /// <summary>
        /// Payment id.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The id of the merchant that requested this payment.
        /// </summary>
        [Required]
        public int MerchantId { get; set; }

        /// <summary>
        /// The card holder name.
        /// </summary>
        [Required]
        public string CardHolderName { get; set; }

        /// <summary>
        /// The masked number of the card used in the payment.
        /// Masking as per PCI (https://security.stackexchange.com/a/145079)
        /// </summary>
        [Required]
        public string MaskedCardNumber { get; set; }

        /// <summary>
        /// The expiry date of the card.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// The amount of the payment.
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        /// <summary>
        /// The ISO 4217 currency code of the payment.
        /// </summary>
        [Required]
        [RegularExpression(@"^[A-Z]{3}$")]
        public string Currency { get; set; }

        /// <summary>
        /// Status of the payment - either Approved or Denied.
        /// </summary>
        [Required]        
        public string Status { get; set; }

        /// <summary>
        /// Timestamp of when the payment was processed by the acquiring bank.
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; }
    }
}
