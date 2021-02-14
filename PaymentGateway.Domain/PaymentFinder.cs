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

        public async Task<Payment> Find(int merchantId, string acquiringBankPaymentId)
        {
            return await this.mediator.Send(new FindPayment(merchantId, acquiringBankPaymentId));
        }
    }
}
