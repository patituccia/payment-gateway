using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PaymentGateway.Controllers;
using PaymentGateway.Domain;
using PaymentGateway.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Tests
{
    public class PaymentsControllerTests
    {
        [Fact]
        public async Task Process_PaymentRequest_ReturnsPaymentResponse()
        {
            // Arrange
            var logger = Substitute.For<ILogger<PaymentsController>>();
            var paymentRequestProcessorSub = Substitute.For<IPaymentRequestProcessor>();
            var paymentRequestDto = new PaymentRequestDto
            {
                MerchantId = 0,
                CardHolderName = "John Smith",
                CardNumber = "1234 1234 1234 1234",
                Amount = 100.00M,
                Currency = "GBP",
                CVV = "123",
                ExpiryDate = DateTime.Now
            };
            var money = new Money(paymentRequestDto.Amount, paymentRequestDto.Currency);
            var acquiringBankPaymentId = Guid.NewGuid().ToString();
            var timestamp = DateTime.Now;
            paymentRequestProcessorSub
                .Process(Arg.Is<PaymentRequest>(p => p.CardNumber == paymentRequestDto.CardNumber.Replace(" ", string.Empty)))
                .Returns(Task.FromResult(new Payment(1,
                                                     100,
                                                     "John Smith",
                                                     "123412******1234",
                                                     paymentRequestDto.ExpiryDate,
                                                     money,
                                                     acquiringBankPaymentId,
                                                     PaymentStatus.Approved,
                                                     timestamp)));
            var paymentFinderSub = Substitute.For<IPaymentFinder>();
            var controller = new PaymentsController(paymentRequestProcessorSub, paymentFinderSub, logger);

            // Act
            var response = await controller.Process(paymentRequestDto);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.IsType<OkObjectResult>(response.Result);
            var result = (OkObjectResult)response.Result;
            var value = (PaymentResponseDto)result.Value;
            Assert.Equal(value.AcquiringBankPaymentId, acquiringBankPaymentId);
            Assert.Equal(value.Status, PaymentStatus.Approved.ToString());
            Assert.Equal(value.Timestamp, timestamp);
        }

        [Fact]
        public async Task Find_AcquiringBankPaymentId_ReturnsPayment()
        {
            // Arrange
            var logger = Substitute.For<ILogger<PaymentsController>>();
            var paymentRequestProcessorSub = Substitute.For<IPaymentRequestProcessor>();
            var paymentFinderSub = Substitute.For<IPaymentFinder>();
            var acquiringBankPaymentId = Guid.NewGuid().ToString();
            var timestamp = DateTime.Now;
            const string MaskedCardNumber = "123456******1234";
            var expiryDate = DateTime.Now;
            const string cardHolderName = "John Smith";
            paymentFinderSub
                .Find(100, acquiringBankPaymentId)
                .Returns(new Payment(1,
                                     100,
                                     cardHolderName,
                                     MaskedCardNumber,
                                     expiryDate,
                                     new Money(100, "GBP"),
                                     acquiringBankPaymentId,
                                     PaymentStatus.Denied,
                                     timestamp));
            var controller = new PaymentsController(paymentRequestProcessorSub, paymentFinderSub, logger);

            // Act
            var response = await controller.Find(100, acquiringBankPaymentId);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.IsType<OkObjectResult>(response.Result);
            var result = (OkObjectResult)response.Result;
            var value = (PaymentDto)result.Value;
            Assert.Equal(1, value.Id);
            Assert.Equal(100, value.MerchantId);
            Assert.Equal(cardHolderName, value.CardHolderName);
            Assert.Equal(MaskedCardNumber, value.MaskedCardNumber);
            Assert.Equal(expiryDate, value.ExpiryDate);
            Assert.Equal(100M, value.Amount);
            Assert.Equal("GBP", value.Currency);
            Assert.Equal(PaymentStatus.Denied.ToString(), value.Status);
            Assert.Equal(timestamp, value.Timestamp);
        }

        [Fact]
        public async Task Find_AcquiringBankPaymentId_ReturnsNotFound()
        {
            // Arrange
            var logger = Substitute.For<ILogger<PaymentsController>>();
            var paymentRequestProcessorSub = Substitute.For<IPaymentRequestProcessor>();
            var paymentFinderSub = Substitute.For<IPaymentFinder>();
            var acquiringBankPaymentId = Guid.NewGuid().ToString();
            paymentFinderSub.Find(100, acquiringBankPaymentId).Returns((Payment)null);
            var controller = new PaymentsController(paymentRequestProcessorSub, paymentFinderSub, logger);

            // Act
            var response = await controller.Find(100, acquiringBankPaymentId);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.IsType<NotFoundResult>(response.Result);
        }
    }
}
