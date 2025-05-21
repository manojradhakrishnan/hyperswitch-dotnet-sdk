using System.Threading;
using System.Threading.Tasks;
using HyperSwitch.SDK.Client;
using HyperSwitch.SDK.Models;

namespace HyperSwitch.SDK.Api
{
    /// <summary>
    /// API for managing payments
    /// </summary>
    public class PaymentsApi
    {
        private const string BasePath = "payments";
        private readonly HyperSwitchClient _client;

        /// <summary>
        /// Initializes a new instance of the PaymentsApi class
        /// </summary>
        /// <param name="client">The client to use for API requests</param>
        public PaymentsApi(HyperSwitchClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Creates a new payment
        /// </summary>
        /// <param name="request">The payment request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created payment</returns>
        public async Task<Payment> CreatePaymentAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default)
        {
            return await _client.PostAsync<Payment>(BasePath, request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves a payment by ID
        /// </summary>
        /// <param name="paymentId">The ID of the payment to retrieve</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The retrieved payment</returns>
        public async Task<Payment> RetrievePaymentAsync(string paymentId, CancellationToken cancellationToken = default)
        {
            return await _client.GetAsync<Payment>($"{BasePath}/{paymentId}", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates a payment
        /// </summary>
        /// <param name="paymentId">The ID of the payment to update</param>
        /// <param name="request">The update request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated payment</returns>
        public async Task<Payment> UpdatePaymentAsync(string paymentId, object request, CancellationToken cancellationToken = default)
        {
            return await _client.PutAsync<Payment>($"{BasePath}/{paymentId}", request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Confirms a payment
        /// </summary>
        /// <param name="paymentId">The ID of the payment to confirm</param>
        /// <param name="request">The confirm request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The confirmed payment</returns>
        public async Task<Payment> ConfirmPaymentAsync(string paymentId, object request, CancellationToken cancellationToken = default)
        {
            return await _client.PostAsync<Payment>($"{BasePath}/{paymentId}/confirm", request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Captures a payment
        /// </summary>
        /// <param name="paymentId">The ID of the payment to capture</param>
        /// <param name="request">The capture request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The captured payment</returns>
        public async Task<Payment> CapturePaymentAsync(string paymentId, object request, CancellationToken cancellationToken = default)
        {
            return await _client.PostAsync<Payment>($"{BasePath}/{paymentId}/capture", request, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels a payment
        /// </summary>
        /// <param name="paymentId">The ID of the payment to cancel</param>
        /// <param name="request">The cancel request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The canceled payment</returns>
        public async Task<Payment> CancelPaymentAsync(string paymentId, object request, CancellationToken cancellationToken = default)
        {
            return await _client.PostAsync<Payment>($"{BasePath}/{paymentId}/cancel", request, cancellationToken).ConfigureAwait(false);
        }
    }
} 