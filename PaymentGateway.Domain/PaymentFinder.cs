using MediatR;
using PaymentGateway.Domain.Events;
using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    public class PaymentFinder : IPaymentFinder
    {
        private readonly IMediator mediator;

        public PaymentFinder(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<Payment> Find(string acquiringBankIdentifier)
        {
            return await this.mediator.Send(new FindPayment(acquiringBankIdentifier));
        }
    }
}
