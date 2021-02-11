using FluentAssertions;
using MediatR;
using NSubstitute;
using PaymentGateway.Domain.Events;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    public class PaymentRequestProcessorTests
    {
        [Fact]
        public async Task Process_ValidRequest_ReturnsPayment()
        {
            // Arrange
            var acquiringBank = Substitute.For<IAcquiringBank>();
            var mediator = Substitute.For<IMediator>();
            var acquiringBankPaymentId = Guid.NewGuid().ToString();
            var dateTime = DateTime.Now;
            var money = new Money(100M, "GBP");
            var maskedCardNumber = "123412******1234";
            var paymentRequest = new PaymentRequest(1, "John Smith", "1234 1234 1234 1234", dateTime, money, "123");
            var paymentResponse = new PaymentResponse(acquiringBankPaymentId, PaymentStatus.Approved);
            var payment = new Payment(1, maskedCardNumber, dateTime, money, acquiringBankPaymentId, PaymentStatus.Approved);
            acquiringBank.Process(Arg.Is<PaymentRequest>(pr => pr.CardNumber == "1234123412341234" && pr.CVV == "123")).Returns(paymentResponse);
            mediator
                .Send(Arg.Is<FindMerchant>(f => f.Id == 1))
                .Returns(Task.FromResult(new Merchant(1, "Merchant")));
            mediator
                .Send(Arg.Is<SavePayment>(s => s.Request.CardNumber == "1234123412341234" && s.Request.CVV == "123" && s.Response.AcquiringBankPaymentId == acquiringBankPaymentId))
                .Returns(Task.FromResult(payment));
            var paymentRequestProcessor = new PaymentRequestProcessor(acquiringBank, mediator);

            // Act 
            var result = await paymentRequestProcessor.Process(paymentRequest);

            // Assert
            result.Id.Should().Be(1);
            result.MaskedCardNumber.Should().Be(maskedCardNumber);
            result.ExpiryDate.Should().Be(dateTime);
            result.Money.Amount.Should().Be(money.Amount);
            result.Money.Currency.Should().Be(money.Currency);
            result.AcquiringBankPaymentId.Should().Be(acquiringBankPaymentId);
            result.Status.Should().Be(PaymentStatus.Approved);
        }

        [Fact]
        public async Task Process_AlreadyProcessedRequest_ThrowsException()
        {
            // Arrange
            var acquiringBank = Substitute.For<IAcquiringBank>();
            var mediator = Substitute.For<IMediator>();
            var dateTime = DateTime.Now;
            var money = new Money(100M, "GBP");
            var paymentRequest = new PaymentRequest(1, "John Smith", "1234 1234 1234 1234", dateTime, money, "123") { IsProcessed = true };
            var paymentRequestProcessor = new PaymentRequestProcessor(acquiringBank, mediator);

            // Act
            Func<Task> act = async () => await paymentRequestProcessor.Process(paymentRequest);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Request have been processed already.");
        }

        [Fact]
        public async Task Process_UnknownMerchant_ThrowsException()
        {
            // Arrange
            var acquiringBank = Substitute.For<IAcquiringBank>();
            var mediator = Substitute.For<IMediator>();
            var dateTime = DateTime.Now;
            var money = new Money(100M, "GBP");
            var paymentRequest = new PaymentRequest(1, "John Smith", "1234 1234 1234 1234", dateTime, money, "123");
            var paymentRequestProcessor = new PaymentRequestProcessor(acquiringBank, mediator);
            mediator
                .Send(Arg.Is<FindMerchant>(f => f.Id == 1))
                .Returns(Task.FromResult<Merchant>(null));

            // Act
            Func<Task> act = async () => await paymentRequestProcessor.Process(paymentRequest);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Merchant with Id: 1 not found.");
        }
    }
}
