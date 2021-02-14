using MediatR;

namespace PaymentGateway.Domain.Events
{
    /// <summary>
    /// Domain event (request) for a Payment with a specified Acquiring Bank Payment Id.
    /// </summary>
    /// <remarks>
    /// Implementers should return null if Payment not found.
    /// </remarks>
    public class FindPayment : IRequest<Payment>
    {
        public FindPayment(int merchantId, string acquiringBankPaymentId)
        {
            if (string.IsNullOrEmpty(acquiringBankPaymentId))
            {
                throw new System.ArgumentException($"'{nameof(acquiringBankPaymentId)}' cannot be null or empty", nameof(acquiringBankPaymentId));
            }

            this.MerchantId = merchantId;
            this.AcquiringPaymentBankId = acquiringBankPaymentId;
        }

        public int MerchantId { get; }

        public string AcquiringPaymentBankId { get; }
    }
}
