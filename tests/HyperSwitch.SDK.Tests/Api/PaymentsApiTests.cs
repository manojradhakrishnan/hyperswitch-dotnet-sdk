using System.Threading;
using System.Threading.Tasks;
using HyperSwitch.SDK.Api;
using HyperSwitch.SDK.Client;
using HyperSwitch.SDK.Models;
using Moq;
using Xunit;

namespace HyperSwitch.SDK.Tests.Api
{
    public class PaymentsApiTests
    {
        private readonly Mock<HyperSwitchClient> _mockClient;
        private readonly PaymentsApi _paymentsApi;

        public PaymentsApiTests()
        {
            _mockClient = new Mock<HyperSwitchClient>(MockBehavior.Strict, new HyperSwitchConfiguration("test_key"));
            _paymentsApi = new PaymentsApi(_mockClient.Object);
        }

        [Fact]
        public async Task CreatePaymentAsync_CallsPostAsync()
        {
            // Arrange
            var request = new CreatePaymentRequest
            {
                Amount = 1000,
                Currency = "USD"
            };
            var expectedPayment = new Payment { Id = "pay_123", Amount = 1000, Currency = "USD" };

            _mockClient
                .Setup(c => c.PostAsync<Payment>("payments", request, CancellationToken.None))
                .ReturnsAsync(expectedPayment);

            // Act
            var result = await _paymentsApi.CreatePaymentAsync(request);

            // Assert
            Assert.Equal(expectedPayment.Id, result.Id);
            Assert.Equal(expectedPayment.Amount, result.Amount);
            Assert.Equal(expectedPayment.Currency, result.Currency);
            _mockClient.Verify(c => c.PostAsync<Payment>("payments", request, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task RetrievePaymentAsync_CallsGetAsync()
        {
            // Arrange
            var paymentId = "pay_123";
            var expectedPayment = new Payment { Id = paymentId, Amount = 1000, Currency = "USD" };

            _mockClient
                .Setup(c => c.GetAsync<Payment>($"payments/{paymentId}", CancellationToken.None))
                .ReturnsAsync(expectedPayment);

            // Act
            var result = await _paymentsApi.RetrievePaymentAsync(paymentId);

            // Assert
            Assert.Equal(expectedPayment.Id, result.Id);
            Assert.Equal(expectedPayment.Amount, result.Amount);
            Assert.Equal(expectedPayment.Currency, result.Currency);
            _mockClient.Verify(c => c.GetAsync<Payment>($"payments/{paymentId}", CancellationToken.None), Times.Once);
        }
    }
} 