using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class PaymentResponseDto
    {
        [Required]
        public string AcquiringBankPaymentId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
