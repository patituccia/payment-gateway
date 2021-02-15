using System;
using System.Threading.Tasks;
using PaymentGateway.Domain;

namespace PaymentGateway.AcquiringBank.Mock
{
    public class MockBank : IAcquiringBank
    {
        private readonly Random delayGenerator =  new Random();

        public async Task<PaymentResponse> Process(PaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var delay = delayGenerator.Next(100, 1000);
            var status = delay > 800 ? PaymentStatus.Denied : PaymentStatus.Approved;
            await Task.Delay(delay);

            return new PaymentResponse(Guid.NewGuid().ToString(), status, DateTime.Now);
        }
    }
}
