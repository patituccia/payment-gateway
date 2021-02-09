using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class PaymentRequestDto
    {
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

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
        [RegularExpression(@"^[0-9]{3,4}$")]
        public string CVV { get; set;  }
    }
}
