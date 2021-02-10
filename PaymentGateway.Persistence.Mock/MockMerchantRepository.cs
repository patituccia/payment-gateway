using MediatR;
using PaymentGateway.Domain;
using PaymentGateway.Domain.Events;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Persistence.Mock
{
    public class MockMerchantRepository : IRequestHandler<SaveMerchant, Merchant>, IRequestHandler<FindMerchant, Merchant>
    {
        private static int NextMerchantId = 0;

        private static readonly ConcurrentDictionary<int, Merchant> MerchantDb = new ConcurrentDictionary<int, Merchant>();

        public MockMerchantRepository()
        {
            this.PopulateMerchants();
        }

        public Task<Merchant> Handle(SaveMerchant request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return Task.FromResult(this.AddMerchant(request.Merchant.Name));
        }

        public Task<Merchant> Handle(FindMerchant request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (MerchantDb.TryGetValue(request.Id, out var merchant))
            {
                return Task.FromResult(merchant);
            }

            return Task.FromResult<Merchant>(null);
        }

        private void PopulateMerchants()
        {
            this.AddMerchant("Apple");
            this.AddMerchant("Microsoft");
            this.AddMerchant("Amazon");
        }

        private Merchant AddMerchant(string name)
        {
            var id = this.GetNextId();
            var merchant = new Merchant(id, name);
            if (!MerchantDb.TryAdd(id, merchant))
            {
                throw new InvalidOperationException($"The key {id} already exists in the dictionary");
            }

            return merchant;
        }

        private int GetNextId()
        {
            return Interlocked.Increment(ref NextMerchantId);
        }
    }
}
