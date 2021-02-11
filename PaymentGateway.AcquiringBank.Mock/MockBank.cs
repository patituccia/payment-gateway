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

            var status = request.CardNumber.StartsWith("1") ? PaymentStatus.Denied : PaymentStatus.Approved;

            await Task.Delay(delayGenerator.Next(100, 1000));

            return new PaymentResponse(Guid.NewGuid().ToString(), status, DateTime.Now);
        }
    }
}
