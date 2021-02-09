using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain;
using PaymentGateway.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRequestFactory paymentRequestFactory;
        private readonly IPaymentFinder paymentFinder;

        public PaymentsController(IPaymentRequestFactory paymentRequestFactory, IPaymentFinder paymentFinder)
        {
            this.paymentRequestFactory = paymentRequestFactory;
            this.paymentFinder = paymentFinder;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResponseDto>> Process(PaymentRequestDto paymentRequestDto)
        {
            var paymentRequest = this.paymentRequestFactory.Create(
                paymentRequestDto.CardNumber,
                paymentRequestDto.ExpiryDate,
                new Money(paymentRequestDto.Amount, paymentRequestDto.Currency),
                paymentRequestDto.CVV);

            var payment = await paymentRequest.Process();

            var result = new PaymentResponseDto { AcquiringBankId = payment.AcquiringBankIdentifier, Status = payment.Status.ToString() };

            return CreatedAtAction(nameof(Process), result);
        }
        
        [HttpGet]
        public async Task<ActionResult<PaymentDto>> Find([Required]string acquiringBankId)
        {
            var payment = await this.paymentFinder.Find(acquiringBankId);

            var result = new PaymentDto
            {
                MaskedCardNumber = payment.MaskedCardNumber,
                Amount = payment.Money.Amount,
                Currency = payment.Money.Currency,
                ExpiryDate = payment.ExpiryDate,
                Status = payment.Status.ToString()
            };

            return result;
        }
    }
}
