using MediatR;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Persistence.SqlLite
{
    public class MerchantRepository : IRequestHandler<SaveMerchant, Merchant>, IRequestHandler<FindMerchant, Merchant>
    {
        public async Task<Merchant> Handle(SaveMerchant request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await AddMerchant(request);
        }

        public async Task<Merchant> Handle(FindMerchant request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var db = new PaymentGatewayDbContext())
            {
                return await db.Merchants.FindAsync(request.Id);
            }
        }

        private static async Task<Merchant> AddMerchant(SaveMerchant request)
        {
            using (var db = new PaymentGatewayDbContext())
            {
                var newMerchant = new Merchant(0, request.Merchant.Name);
                db.Add(newMerchant);
                await db.SaveChangesAsync();

                return newMerchant;
            }
        }
    }
}
