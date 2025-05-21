using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace HyperSwitch.SDK.Models
{
    /// <summary>
    /// Represents a payment response
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// The unique identifier for the payment
        /// </summary>
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// The amount of the payment in the smallest currency unit
        /// </summary>
        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// The three-letter ISO currency code
        /// </summary>
        [JsonProperty("currency")]
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// The status of the payment
        /// </summary>
        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// The authentication type used for the payment
        /// </summary>
        [JsonProperty("authentication_type")]
        [JsonPropertyName("authentication_type")]
        public string? AuthenticationType { get; set; }

        /// <summary>
        /// The customer ID associated with the payment
        /// </summary>
        [JsonProperty("customer_id")]
        [JsonPropertyName("customer_id")]
        public string? CustomerId { get; set; }

        /// <summary>
        /// The payment method type used
        /// </summary>
        [JsonProperty("payment_method")]
        [JsonPropertyName("payment_method")]
        public string? PaymentMethod { get; set; }

        /// <summary>
        /// The client secret for client-side confirmation
        /// </summary>
        [JsonProperty("client_secret")]
        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }
        
        /// <summary>
        /// The amount that can be captured
        /// </summary>
        [JsonProperty("amount_capturable")]
        [JsonPropertyName("amount_capturable")]
        public int? AmountCapturable { get; set; }
        
        /// <summary>
        /// Whether the payment requires additional actions
        /// </summary>
        [JsonProperty("requires_customer_action")]
        [JsonPropertyName("requires_customer_action")]
        public bool? RequiresCustomerAction { get; set; }

        /// <summary>
        /// The URL for handling customer actions
        /// </summary>
        [JsonProperty("next_action")]
        [JsonPropertyName("next_action")]
        public Dictionary<string, object>? NextAction { get; set; }

        /// <summary>
        /// The date and time when the payment was created
        /// </summary>
        [JsonProperty("created")]
        [JsonPropertyName("created")]
        public DateTime? Created { get; set; }

        /// <summary>
        /// The date and time when the payment was last modified
        /// </summary>
        [JsonProperty("modified")]
        [JsonPropertyName("modified")]
        public DateTime? Modified { get; set; }

        /// <summary>
        /// Metadata associated with the payment
        /// </summary>
        [JsonProperty("metadata")]
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// The connector used for processing the payment
        /// </summary>
        [JsonProperty("connector")]
        [JsonPropertyName("connector")]
        public string? Connector { get; set; }
    }
} 