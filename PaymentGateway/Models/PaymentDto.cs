using System;

namespace PaymentGateway.Models
{
    public class PaymentDto
    {
        public string MaskedCardNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
        
        public string Status { get; set; }
    }
}
