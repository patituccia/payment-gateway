using MediatR;

namespace PaymentGateway.Domain.Events
{
    public class SavePayment : IRequest<Payment>
    {
        public SavePayment(PaymentRequest request, PaymentResponse response)
        {
            this.Request = request ?? throw new System.ArgumentNullException(nameof(request));
            this.Response = response ?? throw new System.ArgumentNullException(nameof(response));
        }

        public PaymentRequest Request { get; }

        public PaymentResponse Response { get; }
    }
}
