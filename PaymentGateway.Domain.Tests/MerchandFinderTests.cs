using FluentAssertions;
using MediatR;
using NSubstitute;
using PaymentGateway.Domain.Events;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Domain.Tests
{
    public class MerchandFinderTests
    {
        [Fact]
        public async Task Find_Id_ReturnsMerchant()
        {
            // Arrange
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Is<FindMerchant>(f => f.Id == 1)).Returns(new Merchant(1, "Merchant"));
            var merchantFinder = new MerchantFinder(mediator);

            // Act
            var result = await merchantFinder.Find(1);

            // Assert
            result.Id.Should().Be(1);
            result.Name.Should().Be("Merchant");
        }
    }
}
