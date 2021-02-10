using MediatR;
using System;

namespace PaymentGateway.Domain.Events
{
    public class SaveMerchant : IRequest<Merchant>
    {
        public SaveMerchant(Merchant merchant)
        {
            this.Merchant = merchant ?? throw new System.ArgumentNullException(nameof(merchant));
            if (string.IsNullOrEmpty(merchant.Name))
            {
                throw new InvalidOperationException("Merchant name cannot be null or empty");
            }
        }

        public Merchant Merchant { get; }
    }
}
