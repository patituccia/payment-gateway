using Microsoft.AspNetCore.Mvc;
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
            paymentRequestProcessorSub
                .Process(Arg.Is<PaymentRequest>(p => p.CardNumber == paymentRequestDto.CardNumber.Replace(" ", string.Empty)))
                .Returns(Task.FromResult(new Payment(1, "123412******1234", paymentRequestDto.ExpiryDate, money, acquiringBankPaymentId, PaymentStatus.Approved)));
            var paymentFinderSub = Substitute.For<IPaymentFinder>();
            var controller = new PaymentsController(paymentRequestProcessorSub, paymentFinderSub);

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
        }

        [Fact]
        public async Task Find_AcquiringBankPaymentId_ReturnsPayment()
        {
            // Arrange
            var paymentRequestProcessorSub = Substitute.For<IPaymentRequestProcessor>();
            var paymentFinderSub = Substitute.For<IPaymentFinder>();
            var acquiringBankPaymentId = Guid.NewGuid().ToString();
            const string MaskedCardNumber = "123456******1234";
            var expiryDate = DateTime.Now;
            paymentFinderSub.Find(acquiringBankPaymentId).Returns(new Payment(1, MaskedCardNumber, expiryDate, new Money(100, "GBP"), acquiringBankPaymentId, PaymentStatus.Denied));
            var controller = new PaymentsController(paymentRequestProcessorSub, paymentFinderSub);

            // Act
            var response = await controller.Find(acquiringBankPaymentId);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.IsType<OkObjectResult>(response.Result);
            var result = (OkObjectResult)response.Result;
            var value = (PaymentDto)result.Value;
            Assert.Equal(MaskedCardNumber, value.MaskedCardNumber);
            Assert.Equal(expiryDate, value.ExpiryDate);
            Assert.Equal(100M, value.Amount);
            Assert.Equal("GBP", value.Currency);
            Assert.Equal(PaymentStatus.Denied.ToString(), value.Status);
        }

        [Fact]
        public async Task Find_AcquiringBankPaymentId_ReturnsNotFound()
        {
            // Arrange
            var paymentRequestProcessorSub = Substitute.For<IPaymentRequestProcessor>();
            var paymentFinderSub = Substitute.For<IPaymentFinder>();
            var acquiringBankPaymentId = Guid.NewGuid().ToString();
            paymentFinderSub.Find(acquiringBankPaymentId).Returns((Payment)null);
            var controller = new PaymentsController(paymentRequestProcessorSub, paymentFinderSub);

            // Act
            var response = await controller.Find(acquiringBankPaymentId);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.IsType<NotFoundResult>(response.Result);
        }
    }
}
