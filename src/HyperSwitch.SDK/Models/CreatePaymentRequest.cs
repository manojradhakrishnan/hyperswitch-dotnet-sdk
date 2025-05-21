using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace HyperSwitch.SDK.Models
{
    /// <summary>
    /// Represents a request to create a payment
    /// </summary>
    public class CreatePaymentRequest
    {
        /// <summary>
        /// The amount to be charged in the smallest currency unit (e.g., cents for USD)
        /// </summary>
        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// The three-letter ISO currency code
        /// </summary>
        [JsonProperty("currency")]
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// Whether to automatically confirm the payment
        /// </summary>
        [JsonProperty("confirm")]
        [JsonPropertyName("confirm")]
        public bool? Confirm { get; set; }

        /// <summary>
        /// The capture method for the payment
        /// </summary>
        [JsonProperty("capture_method")]
        [JsonPropertyName("capture_method")]
        public string? CaptureMethod { get; set; }

        /// <summary>
        /// The date and time when the payment should be captured
        /// </summary>
        [JsonProperty("capture_on")]
        [JsonPropertyName("capture_on")]
        public DateTime? CaptureOn { get; set; }

        /// <summary>
        /// The amount to capture if different from the original amount
        /// </summary>
        [JsonProperty("amount_to_capture")]
        [JsonPropertyName("amount_to_capture")]
        public int? AmountToCapture { get; set; }

        /// <summary>
        /// The profile ID associated with this payment
        /// </summary>
        [JsonProperty("profile_id")]
        [JsonPropertyName("profile_id")]
        public string? ProfileId { get; set; }

        /// <summary>
        /// The business country code
        /// </summary>
        [JsonProperty("business_country")]
        [JsonPropertyName("business_country")]
        public string? BusinessCountry { get; set; }

        /// <summary>
        /// The business label
        /// </summary>
        [JsonProperty("business_label")]
        [JsonPropertyName("business_label")]
        public string? BusinessLabel { get; set; }

        /// <summary>
        /// The customer ID
        /// </summary>
        [JsonProperty("customer_id")]
        [JsonPropertyName("customer_id")]
        public string? CustomerId { get; set; }

        /// <summary>
        /// The customer's email address
        /// </summary>
        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// The customer's name
        /// </summary>
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The customer's phone number
        /// </summary>
        [JsonProperty("phone")]
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// The customer's phone country code
        /// </summary>
        [JsonProperty("phone_country_code")]
        [JsonPropertyName("phone_country_code")]
        public string? PhoneCountryCode { get; set; }

        /// <summary>
        /// A description of the payment
        /// </summary>
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The authentication type for the payment
        /// </summary>
        [JsonProperty("authentication_type")]
        [JsonPropertyName("authentication_type")]
        public string? AuthenticationType { get; set; }

        /// <summary>
        /// The URL to redirect to after the payment
        /// </summary>
        [JsonProperty("return_url")]
        [JsonPropertyName("return_url")]
        public string? ReturnUrl { get; set; }

        /// <summary>
        /// The payment method type (e.g., "card", "paypal", etc.)
        /// </summary>
        [JsonProperty("payment_method")]
        [JsonPropertyName("payment_method")]
        public string? PaymentMethod { get; set; }

        /// <summary>
        /// The specific payment method type (e.g., "credit", "debit", etc.)
        /// </summary>
        [JsonProperty("payment_method_type")]
        [JsonPropertyName("payment_method_type")]
        public string? PaymentMethodType { get; set; }

        /// <summary>
        /// The payment method data
        /// </summary>
        [JsonProperty("payment_method_data")]
        [JsonPropertyName("payment_method_data")]
        public PaymentMethodData? PaymentMethodData { get; set; }

        /// <summary>
        /// The billing information
        /// </summary>
        [JsonProperty("billing")]
        [JsonPropertyName("billing")]
        public ContactInformation? Billing { get; set; }

        /// <summary>
        /// The shipping information
        /// </summary>
        [JsonProperty("shipping")]
        [JsonPropertyName("shipping")]
        public ContactInformation? Shipping { get; set; }

        /// <summary>
        /// The statement descriptor name
        /// </summary>
        [JsonProperty("statement_descriptor_name")]
        [JsonPropertyName("statement_descriptor_name")]
        public string? StatementDescriptorName { get; set; }

        /// <summary>
        /// The statement descriptor suffix
        /// </summary>
        [JsonProperty("statement_descriptor_suffix")]
        [JsonPropertyName("statement_descriptor_suffix")]
        public string? StatementDescriptorSuffix { get; set; }

        /// <summary>
        /// Additional metadata for the payment
        /// </summary>
        [JsonProperty("metadata")]
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }
    }
} 