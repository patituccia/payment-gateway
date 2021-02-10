using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain;
using PaymentGateway.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace PaymentGateway.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRequestProcessor paymentRequestProcessor;
        private readonly IPaymentFinder paymentFinder;

        public PaymentsController(IPaymentRequestProcessor paymentRequestProcessor, IPaymentFinder paymentFinder)
        {
            this.paymentRequestProcessor = paymentRequestProcessor;
            this.paymentFinder = paymentFinder;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResponseDto>> Process(PaymentRequestDto paymentRequestDto)
        {
            var paymentRequest = new PaymentRequest(
                paymentRequestDto.MerchantId,
                paymentRequestDto.CardHolderName,
                paymentRequestDto.CardNumber,
                paymentRequestDto.ExpiryDate,
                new Money(paymentRequestDto.Amount, paymentRequestDto.Currency),
                paymentRequestDto.CVV);

            var payment = await paymentRequestProcessor.Process(paymentRequest);

            var result = new PaymentResponseDto { AcquiringBankPaymentId = payment.AcquiringBankPaymentId, Status = payment.Status.ToString() };

            return this.Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PaymentDto>> Find([Required]string acquiringBankPaymentId)
        {
            var payment = await this.paymentFinder.Find(acquiringBankPaymentId);

            if (payment == null)
            {
                return this.NotFound();
            }

            var result = new PaymentDto
            {
                MaskedCardNumber = payment.MaskedCardNumber,
                Amount = payment.Money.Amount,
                Currency = payment.Money.Currency,
                ExpiryDate = payment.ExpiryDate,
                Status = payment.Status.ToString()
            };

            return this.Ok(result);
        }
    }
}
