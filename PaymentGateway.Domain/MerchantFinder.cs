using MediatR;
using PaymentGateway.Domain.Events;
using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    public class MerchantFinder : IMerchantFinder
    {
        private readonly IMediator mediator;

        public MerchantFinder(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<Merchant> Find(int id)
        {
            return await this.mediator.Send(new FindMerchant(id));
        }
    }
}
