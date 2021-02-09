using System;
using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    public interface IPaymentRequestProcessor
    {
        Task<Payment> Process(PaymentRequest request);
    }
}
