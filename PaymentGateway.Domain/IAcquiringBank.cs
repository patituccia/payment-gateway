using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    /// <summary>
    /// Represents an Acquiring Banks that will process payments
    /// </summary>
    public interface IAcquiringBank
    {
        Task<PaymentResponse> Process(PaymentRequest request);
    }
}
