namespace PaymentGateway.Domain
{
    public class PaymentResponse
    {
        public PaymentResponse(string acquiringBankIdentifier, PaymentStatus status)
        {
            if (string.IsNullOrEmpty(acquiringBankIdentifier))
            {
                throw new System.ArgumentException($"'{nameof(acquiringBankIdentifier)}' cannot be null or empty", nameof(acquiringBankIdentifier));
            }

            this.AcquiringBankIdentifier = acquiringBankIdentifier;
            this.Status = status;
        }

        public string AcquiringBankIdentifier { get; set; }

        public PaymentStatus Status { get; set; }
    }
}
