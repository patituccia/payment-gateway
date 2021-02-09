using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class PaymentResponseDto
    {
        [Required]
        public string AcquiringBankId { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
