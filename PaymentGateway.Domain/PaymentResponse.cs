namespace PaymentGateway.Domain
{
    public class PaymentResponse
    {
        public PaymentResponse(string acquiringBankPaymentId, PaymentStatus status)
        {
            if (string.IsNullOrEmpty(acquiringBankPaymentId))
            {
                throw new System.ArgumentException($"'{nameof(acquiringBankPaymentId)}' cannot be null or empty", nameof(acquiringBankPaymentId));
            }

            this.AcquiringBankPaymentId = acquiringBankPaymentId;
            this.Status = status;
        }

        public string AcquiringBankPaymentId { get; set; }

        public PaymentStatus Status { get; set; }
    }
}
