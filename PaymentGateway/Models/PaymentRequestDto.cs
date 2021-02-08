using System;

namespace PaymentGateway.Models
{
    public class PaymentRequestDto
    {
        public string CardNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public int CVV { get; set;  }
    }
}
