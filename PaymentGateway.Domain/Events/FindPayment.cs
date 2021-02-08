using MediatR;

namespace PaymentGateway.Domain.Events
{
    public class FindPayment : IRequest<Payment>
    {
        public FindPayment(string acquiringBankIdentifier)
        {
            if (string.IsNullOrEmpty(acquiringBankIdentifier))
            {
                throw new System.ArgumentException($"'{nameof(acquiringBankIdentifier)}' cannot be null or empty", nameof(acquiringBankIdentifier));
            }

            this.AcquiringBankIdentifier = acquiringBankIdentifier;
        }

        public string AcquiringBankIdentifier { get; }
    }
}
