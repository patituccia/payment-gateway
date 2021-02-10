using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    public interface IMerchantFinder
    {
        Task<Merchant> Find(int id);
    }
}
