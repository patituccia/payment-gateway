using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain;
using PaymentGateway.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Controllers
{
    /// <summary>
    /// API for requesting payments to be processed and later retrieved.
    /// </summary>
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRequestProcessor paymentRequestProcessor;
        private readonly IPaymentFinder paymentFinder;
        private readonly ILogger<PaymentsController> logger;

        public PaymentsController(IPaymentRequestProcessor paymentRequestProcessor, IPaymentFinder paymentFinder, ILogger<PaymentsController> logger)
        {
            this.paymentRequestProcessor = paymentRequestProcessor;
            this.paymentFinder = paymentFinder;
            this.logger = logger;
        }

        /// <summary>
        /// Processes a payment request via the corresponding acquiring bank on behalf of the merchant.
        /// </summary>
        /// <param name="paymentRequestDto">The payment request.</param>
        /// <returns>The payment response including the acquiring bank payment id an whether it was approved or denied.</returns>
        [HttpPost]
        public async Task<ActionResult<PaymentResponseDto>> Process(PaymentRequestDto paymentRequestDto)
        {
            try
            {
                var paymentRequest = new PaymentRequest(
                paymentRequestDto.MerchantId,
                paymentRequestDto.CardHolderName,
                paymentRequestDto.CardNumber,
                paymentRequestDto.ExpiryDate,
                new Money(paymentRequestDto.Amount, paymentRequestDto.Currency),
                paymentRequestDto.CVV);

                var payment = await paymentRequestProcessor.Process(paymentRequest);

                var result = new PaymentResponseDto
                {
                    AcquiringBankPaymentId = payment.AcquiringBankPaymentId,
                    Status = payment.Status.ToString(),
                    Timestamp = payment.Timestamp
                };

                return this.Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error processing payment.");
            }

            return this.Problem("Error processing payment.");
        }

        /// <summary>
        /// Finds a previously processed payment.
        /// </summary>
        /// <param name="merchantId">The merchant id.</param>
        /// <param name="acquiringBankPaymentId">The acquiring bank unique payment id.</param>
        /// <returns>The payment (if found), 404 (not found) otherwise.</returns>
        [HttpGet]
        public async Task<ActionResult<PaymentDto>> Find([Required]int merchantId, [Required]string acquiringBankPaymentId)
        {
            try
            {
                var payment = await this.paymentFinder.Find(merchantId, acquiringBankPaymentId);

                if (payment == null)
                {
                    return this.NotFound();
                }

                var result = new PaymentDto
                {
                    Id = payment.Id,
                    MerchantId = payment.MerchantId,
                    CardHolderName = payment.CardHolderName,
                    MaskedCardNumber = payment.MaskedCardNumber,
                    Amount = payment.Money.Amount,
                    Currency = payment.Money.Currency,
                    ExpiryDate = payment.ExpiryDate,
                    Status = payment.Status.ToString(),
                    Timestamp = payment.Timestamp
                };

                return this.Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error finding a previously processed payment.");
            }

            return this.Problem("Error finding a previously processed payment.");
        }
    }
}
