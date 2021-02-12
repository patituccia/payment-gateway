using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    /// <summary>
    /// Represents a payment response issued by an acquiring bank.
    /// </summary>
    public class PaymentResponseDto
    {
        /// <summary>
        /// The unique acquiring bank payment id.
        /// </summary>
        [Required]
        public string AcquiringBankPaymentId { get; set; }

        /// <summary>
        /// The payment status - either Approved or Denied.
        /// </summary>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// The date and time when the payment was processed.
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; }
    }
}
