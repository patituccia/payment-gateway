using MediatR;

namespace PaymentGateway.Domain.Events
{
    /// <summary>
    /// Domain event (request) to find the Merchant with the specified Id.
    /// </summary>
    /// <remarks>
    /// Implementers should return null if Merchant not found.
    /// </remarks>
    public class FindMerchant : IRequest<Merchant>
    {
        public FindMerchant(int Id)
        {
            this.Id = Id;
        }

        public int Id { get; }
    }
}
