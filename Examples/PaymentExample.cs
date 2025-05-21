using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using HyperSwitch.SDK.Client;
using HyperSwitch.SDK.Models;

namespace HyperSwitch.SDK.Examples
{
    public class PaymentExample
    {
        public static async Task Main(string[] args)
        {
            // Replace with your actual API key
            var apiKey = "your_api_key";
            
            // Initialize the client with configuration
            var config = new HyperSwitchConfiguration(apiKey);
            using var client = new HyperSwitchClient(config);
            
            // Enable detailed logging
            client.EnableDetailedLogging = true;
            Console.WriteLine("\n=== DETAILED API LOGGING ENABLED ===\n");
            
            try
            {
                // Generate a unique payment ID to use throughout the flow
                string customPaymentId = $"pay_{Guid.NewGuid().ToString("N").Substring(0, 16)}";
                Console.WriteLine($"\n=== Using custom payment ID: {customPaymentId} ===");
                
                // Step 1: Create a payment (WITHOUT payment method data)
                Console.WriteLine("\n=== Step 1: Creating a payment (WITHOUT payment method data) ===");
                var payment = await CreatePaymentExample(client, customPaymentId);
                Console.WriteLine($"Payment object: {JsonSerializer.Serialize(payment, new JsonSerializerOptions { WriteIndented = true })}");
                
                // Use our custom payment ID for the rest of the flow
                string paymentIdentifier = customPaymentId;
                string clientSecret = payment?.ClientSecret;
                
                if (!string.IsNullOrEmpty(paymentIdentifier))
                {
                    // Step 2: Confirm the payment (WITH payment method data)
                    Console.WriteLine("\n=== Step 2: Confirming payment (WITH payment method data) ===");
                    try {
                        // Try confirming with client_secret first, then fall back to payment_id if that fails
                        Payment confirmedPayment = null;
                        
                        if (!string.IsNullOrEmpty(clientSecret)) {
                            try {
                                Console.WriteLine("Attempting to confirm with client_secret...");
                                confirmedPayment = await ConfirmPaymentWithClientSecretExample(client, clientSecret);
                            }
                            catch (Exception ex) {
                                Console.WriteLine($"Error confirming with client_secret: {ex.Message}");
                                Console.WriteLine("Falling back to payment_id confirmation...");
                                confirmedPayment = await ConfirmPaymentExample(client, paymentIdentifier);
                            }
                        } else {
                            confirmedPayment = await ConfirmPaymentExample(client, paymentIdentifier);
                        }
                        
                        Console.WriteLine($"Confirmed payment status: {confirmedPayment?.Status ?? "Unknown"}");
                        Console.WriteLine($"Confirmed payment object: {JsonSerializer.Serialize(confirmedPayment, new JsonSerializerOptions { WriteIndented = true })}");
                    }
                    catch (Exception ex) {
                        Console.WriteLine($"Error confirming payment: {ex.Message}");
                        Console.WriteLine($"Error details: {ex}");
                    }
                    
                    // Step 3: Retrieve the payment details
                    Console.WriteLine("\n=== Step 3: Retrieving payment details ===");
                    Payment paymentDetails = null;
                    try {
                        paymentDetails = await client.Payments.RetrievePaymentAsync(paymentIdentifier);
                        
                        if (paymentDetails != null) {
                            Console.WriteLine($"Retrieved payment status: {paymentDetails?.Status ?? "Unknown"}");
                            Console.WriteLine($"Retrieved payment object: {JsonSerializer.Serialize(paymentDetails, new JsonSerializerOptions { WriteIndented = true })}");
                        } else {
                            Console.WriteLine("Could not retrieve payment details");
                        }
                    }
                    catch (Exception ex) {
                        Console.WriteLine($"Error retrieving payment: {ex.Message}");
                        Console.WriteLine($"Error details: {ex}");
                    }
                    
                    // Step 4: Check payment status for refund
                    Console.WriteLine("\n=== Step 4: Checking payment status for refund ===");
                    Payment currentPayment = null;
                    try {
                        currentPayment = await client.Payments.RetrievePaymentAsync(paymentIdentifier);
                        Console.WriteLine($"Payment status before refund attempt: {currentPayment?.Status ?? "Unknown"}");
                    }
                    catch (Exception ex) {
                        Console.WriteLine($"Error retrieving payment before refund: {ex.Message}");
                    }
                    
                    // Step 5: Refund the payment (if payment was successful)
                    if (currentPayment?.Status == "succeeded")
                    {
                        Console.WriteLine("\n=== Step 5: Refunding payment ===");
                        try {
                            var refund = await RefundPaymentExample(client, paymentIdentifier);
                            Console.WriteLine($"Refund ID: {refund?.RefundId ?? "Unknown"}");
                            Console.WriteLine($"Refund status: {refund?.Status ?? "Unknown"}");
                            Console.WriteLine($"Refund amount: {(refund?.Amount ?? 0)/100.0} {refund?.Currency ?? "Unknown"}");
                            Console.WriteLine($"Refund object: {JsonSerializer.Serialize(refund, new JsonSerializerOptions { WriteIndented = true })}");
                        }
                        catch (Exception ex) {
                            Console.WriteLine($"Error refunding payment: {ex.Message}");
                            Console.WriteLine($"Error details: {ex}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n=== Step 5: Payment not successful, skipping refund ===");
                        Console.WriteLine($"Current payment status: {currentPayment?.Status ?? "Unknown"}");
                    }
                }
                else {
                    Console.WriteLine("Payment ID is null, cannot proceed with confirmation and refund");
                }
            }
            catch (HyperSwitchApiException ex)
            {
                Console.WriteLine($"API Error: Status {ex.StatusCode}");
                Console.WriteLine($"Response: {ex.ResponseBody}");
                Console.WriteLine($"Error details: {ex}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Error details: {ex}");
            }
            
            // Disable detailed logging for cleaner output at the end
            client.EnableDetailedLogging = false;
            Console.WriteLine("\n=== Test execution complete ===");
        }
        
        // Helper method to extract payment ID from client secret
        private static string ExtractPaymentIdFromClientSecret(string clientSecret)
        {
            if (string.IsNullOrEmpty(clientSecret))
                return null;
                
            // Client secret format is typically: "pay_XXXX_secret_YYYY"
            // Extract the pay_XXXX part
            int secretIndex = clientSecret.IndexOf("_secret_");
            if (secretIndex > 0)
            {
                return clientSecret.Substring(0, secretIndex);
            }
            
            return null;
        }
        
        private static async Task<Payment> CreatePaymentExample(HyperSwitchClient client, string paymentId)
        {
            // Create request WITH payment method data
            var requestDict = new Dictionary<string, object>
            {
                ["amount"] = 2000, // $20.00
                ["currency"] = "USD",
                ["payment_method"] = "card", // Specify the method type
                ["description"] = "Test payment from .NET SDK",
                ["profile_id"] = "pro_IZRGM4Gy1AhKHAn0w7lS", // Replace with your actual profile ID
                ["payment_id"] = paymentId, // Add the payment_id parameter
                
                // Add payment method data as required by the API
                ["payment_method_data"] = new Dictionary<string, object>
                {
                    ["card"] = new Dictionary<string, string>
                    {
                        ["card_number"] = "4242424242424242",
                        ["card_exp_month"] = "12",
                        ["card_exp_year"] = "2025",
                        ["card_cvc"] = "123",
                        ["card_holder_name"] = "Test User"
                    }
                },
                
                ["billing"] = new Dictionary<string, object>
                {
                    ["address"] = new Dictionary<string, string>
                    {
                        ["line1"] = "123 Test St",
                        ["city"] = "Test City",
                        ["state"] = "TS",
                        ["zip"] = "12345",
                        ["country"] = "US",
                        ["first_name"] = "Test",
                        ["last_name"] = "User"
                    },
                    ["phone"] = new Dictionary<string, string>
                    {
                        ["number"] = "5555555555",
                        ["country_code"] = "+1"
                    }
                },
                ["metadata"] = new Dictionary<string, string>
                {
                    { "sdk_test", "true" },
                    { "test_date", DateTime.UtcNow.ToString("o") },
                    { "custom_payment_id", paymentId }
                }
            };
            
            // Direct API call to include the payment_id parameter
            var payment = await client.PostAsync<Payment>("payments", requestDict);
            
            Console.WriteLine($"Created payment with ID: {payment?.Id ?? paymentId}");
            Console.WriteLine($"Amount: {(payment?.Amount ?? 0)/100.0} {payment?.Currency ?? "USD"}");
            Console.WriteLine($"Status: {payment?.Status ?? "Unknown"}");
            
            return payment;
        }
        
        private static async Task<Payment> ConfirmPaymentExample(HyperSwitchClient client, string paymentId)
        {
            // Create a dictionary for the confirm request
            var request = new Dictionary<string, object>
            {
                // Include the payment ID
                ["payment_id"] = paymentId,
                
                // Payment method is required
                ["payment_method"] = "card",
                
                // Add payment method data that is required for confirmation
                ["payment_method_data"] = new Dictionary<string, object>
                {
                    ["card"] = new Dictionary<string, string>
                    {
                        ["card_number"] = "4242424242424242",
                        ["card_exp_month"] = "12",
                        ["card_exp_year"] = "2025",
                        ["card_cvc"] = "123",
                        ["card_holder_name"] = "Test User"
                    }
                },
                
                // Add order details as required by the API (as an array with amount)
                ["order_details"] = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        ["product_id"] = "prod_test123",
                        ["quantity"] = 1,
                        ["product_name"] = "Test Product",
                        ["amount"] = 2000 // Amount in smallest currency unit
                    }
                },
                
                // Additional parameters as needed
                ["metadata"] = new Dictionary<string, string>
                {
                    { "confirmed_by", "payment_example" },
                    { "confirm_time", DateTime.UtcNow.ToString("o") }
                }
            };
            
            // Direct API call to include the payment_id parameter in URL path
            var payment = await client.PostAsync<Payment>($"payments/{paymentId}/confirm", request);
            return payment;
        }
        
        private static async Task<Payment> ConfirmPaymentWithClientSecretExample(HyperSwitchClient client, string clientSecret)
        {
            // Extract payment ID from client secret
            string paymentId = ExtractPaymentIdFromClientSecret(clientSecret);
            
            // Create a dictionary for the confirm request
            var request = new Dictionary<string, object>
            {
                ["payment_id"] = paymentId, // Use payment_id instead of client_secret
                ["payment_method"] = "card",
                
                // Add payment method data that is required for confirmation
                ["payment_method_data"] = new Dictionary<string, object>
                {
                    ["card"] = new Dictionary<string, string>
                    {
                        ["card_number"] = "4242424242424242",
                        ["card_exp_month"] = "12",
                        ["card_exp_year"] = "2025",
                        ["card_cvc"] = "123",
                        ["card_holder_name"] = "Test User"
                    }
                },
                
                // Add order details as required by the API (as an array with amount)
                ["order_details"] = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        ["product_id"] = "prod_test123",
                        ["quantity"] = 1,
                        ["product_name"] = "Test Product",
                        ["amount"] = 2000 // Amount in smallest currency unit
                    }
                }
            };
            
            // Direct API call to use payment_id for confirmation
            var payment = await client.PostAsync<Payment>($"payments/{paymentId}/confirm", request);
            return payment;
        }
        
        private static async Task<RefundResponse> RefundPaymentExample(HyperSwitchClient client, string paymentId)
        {
            // Create a refund request
            var request = new Dictionary<string, object>
            {
                ["payment_id"] = paymentId,
                ["amount"] = 2000, // Same amount as the payment
                ["reason"] = "customer_requested",
                ["metadata"] = new Dictionary<string, string>
                {
                    { "refunded_by", "payment_example" },
                    { "refund_time", DateTime.UtcNow.ToString("o") }
                }
            };
            
            // Call the refund API
            var refundResult = await client.PostAsync<RefundResponse>("refunds", request);
            return refundResult;
        }
    }
    
    // Model for refund response
    public class RefundResponse
    {
        [Newtonsoft.Json.JsonProperty("refund_id")]
        [System.Text.Json.Serialization.JsonPropertyName("refund_id")]
        public string? RefundId { get; set; }
        
        [Newtonsoft.Json.JsonProperty("payment_id")]
        [System.Text.Json.Serialization.JsonPropertyName("payment_id")]
        public string? PaymentId { get; set; }
        
        [Newtonsoft.Json.JsonProperty("amount")]
        [System.Text.Json.Serialization.JsonPropertyName("amount")]
        public int Amount { get; set; }
        
        [Newtonsoft.Json.JsonProperty("currency")]
        [System.Text.Json.Serialization.JsonPropertyName("currency")]
        public string? Currency { get; set; }
        
        [Newtonsoft.Json.JsonProperty("status")]
        [System.Text.Json.Serialization.JsonPropertyName("status")]
        public string? Status { get; set; }
        
        [Newtonsoft.Json.JsonProperty("metadata")]
        [System.Text.Json.Serialization.JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }
        
        [Newtonsoft.Json.JsonProperty("error_message")]
        [System.Text.Json.Serialization.JsonPropertyName("error_message")]
        public string? ErrorMessage { get; set; }
        
        [Newtonsoft.Json.JsonProperty("error_code")]
        [System.Text.Json.Serialization.JsonPropertyName("error_code")]
        public string? ErrorCode { get; set; }
        
        [Newtonsoft.Json.JsonProperty("created_at")]
        [System.Text.Json.Serialization.JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
