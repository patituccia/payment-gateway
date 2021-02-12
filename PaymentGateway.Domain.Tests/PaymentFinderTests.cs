using FluentAssertions;
using MediatR;
using NSubstitute;
using PaymentGateway.Domain.Events;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    public class PaymentFinderTests
    {

        [Fact]
        public async Task Find_AcquiringBankPaymentId_ReturnsPayment()
        {
            // Arrange
            var mediator = Substitute.For<IMediator>();
            var acquitingBankPaymentId = Guid.NewGuid().ToString();
            var time = DateTime.Now;
            const string cardHolderName = "John Smith";
            const string MaskedCardNumber = "123456******1234";
            var amount = 100M;
            var currency = "GBP";
            var timestamp = DateTime.Now;
            mediator
                .Send(Arg.Is<FindPayment>(f => f.AcquiringPaymentBankId == acquitingBankPaymentId))
                .Returns(new Payment(
                    1,
                    100,
                    cardHolderName,
                    MaskedCardNumber,
                    time,
                    new Money(amount, currency),
                    acquitingBankPaymentId,
                    PaymentStatus.Approved,
                    timestamp));
            var merchantFinder = new PaymentFinder(mediator);

            // Act
            var result = await merchantFinder.Find(acquitingBankPaymentId);

            // Assert
            result.Id.Should().Be(1);
            result.MerchantId.Should().Be(100);
            result.CardHolderName.Should().Be(cardHolderName);
            result.MaskedCardNumber.Should().Be(MaskedCardNumber);
            result.ExpiryDate.Should().Be(time);
            result.Money.Amount.Should().Be(amount);
            result.Money.Currency.Should().Be(currency);
            result.AcquiringBankPaymentId.Should().Be(acquitingBankPaymentId);
            result.Status.Should().Be(PaymentStatus.Approved);
            result.Timestamp.Should().Be(timestamp);
        }
    }
}
