using PaymentGateway.Domain;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPaymentGatewayDomain(this IServiceCollection services)
        {
            services.AddScoped<IPaymentFinder, PaymentFinder>();
            services.AddScoped<IMerchantFinder, MerchantFinder>();
            services.AddScoped<IPaymentRequestProcessor, PaymentRequestProcessor>();
        }
    }
}
