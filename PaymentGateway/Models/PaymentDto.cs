using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class PaymentDto
    {
        [Required]
        public string MaskedCardNumber { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{3}$")]
        public string Currency { get; set; }

        [Required]        
        public string Status { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
