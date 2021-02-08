using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    public interface IPaymentFinder
    {
        Task<Payment> Find(string acquiringBankIdentifier);
    }
}
