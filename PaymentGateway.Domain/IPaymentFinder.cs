using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    public interface IPaymentFinder
    {
        Task<Payment> Find(int merchantId, string acquiringBankPaymentId);
    }
}
