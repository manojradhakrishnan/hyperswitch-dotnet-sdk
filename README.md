# HyperSwitch .NET SDK

The official .NET SDK for [HyperSwitch](https://hyperswitch.io), providing a convenient interface to the HyperSwitch Payments API.

## Features

- Simple, consistent interface to HyperSwitch APIs
- Typesafe models for request and response objects
- Async/await support
- Comprehensive error handling
- Support for multiple .NET versions (6.0, 7.0, 8.0, 9.0)

## Installation

The SDK can be installed via NuGet:

```bash
dotnet add package HyperSwitch.SDK
```

## Importing and Integrating the SDK

### Adding to a New Project

1. Create a new .NET project if you don't have one already:
   ```bash
   dotnet new console -n MyPaymentApp
   cd MyPaymentApp
   ```

2. Add the HyperSwitch SDK package:
   ```bash
   dotnet add package HyperSwitch.SDK
   ```

3. Import the necessary namespaces in your code:
   ```csharp
   using HyperSwitch.SDK.Client;
   using HyperSwitch.SDK.Api;
   using HyperSwitch.SDK.Models;
   ```

### Basic Integration Steps

1. **Configure the SDK**

   ```csharp
   // Initialize with your API key
   var config = new HyperSwitchConfiguration("your_api_key_here");
   
   // Create the client
   using var client = new HyperSwitchClient(config);
   ```

2. **Create and Process a Payment**

   ```csharp
   // Generate a unique payment ID
   string paymentId = $"pay_{Guid.NewGuid().ToString("N").Substring(0, 16)}";
   
   // Step 1: Create a payment
   var paymentRequest = new CreatePaymentRequest
   {
       Amount = 1000, // Amount in smallest currency unit (e.g., cents)
       Currency = "USD",
       PaymentMethod = "card",
       Description = "Test payment",
       ProfileId = "your_profile_id" // Replace with your profile ID
   };
   
   var payment = await client.Payments.CreatePaymentAsync(paymentRequest);
   Console.WriteLine($"Payment created with ID: {payment.Id}");
   
   // Step 2: Retrieve payment details
   var paymentDetails = await client.Payments.RetrievePaymentAsync(payment.Id);
   Console.WriteLine($"Payment status: {paymentDetails.Status}");
   
   // Step 3: Confirm the payment with payment method data
   // Use Dictionary<string, object> for direct API requests
   var confirmRequest = new Dictionary<string, object>
   {
       ["payment_method"] = "card",
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
       }
   };
   
   var confirmedPayment = await client.PostAsync<Payment>($"payments/{payment.Id}/confirm", confirmRequest);
   Console.WriteLine($"Payment confirmed with status: {confirmedPayment.Status}");
   ```

3. **Error Handling**

   ```csharp
   try 
   {
       var payment = await client.Payments.CreatePaymentAsync(paymentRequest);
       // Process successful payment
   }
   catch (HyperSwitchApiException ex) 
   {
       // Handle API errors (e.g., authentication, validation)
       Console.WriteLine($"API Error: {ex.StatusCode} - {ex.Message}");
       Console.WriteLine($"Response body: {ex.ResponseBody}");
   }
   catch (Exception ex)
   {
       // Handle other errors (network, serialization, etc.)
       Console.WriteLine($"Error: {ex.Message}");
   }
   ```

4. **Enabling Detailed Logging (Optional)**

   ```csharp
   // Enable detailed API request/response logging
   client.EnableDetailedLogging = true;
   ```

### Advanced Usage - Example Payment Flow

Here's how to implement a complete payment flow in your application:

```csharp
async Task ProcessPayment()
{
    var apiKey = "your_api_key_here";
    var config = new HyperSwitchConfiguration(apiKey);
    using var client = new HyperSwitchClient(config);
    
    try
    {
        // Generate a unique payment ID
        string paymentId = $"pay_{Guid.NewGuid().ToString("N").Substring(0, 16)}";
        
        // Step 1: Create a payment with payment method data
        var request = new Dictionary<string, object>
        {
            ["amount"] = 2000, // $20.00
            ["currency"] = "USD",
            ["payment_method"] = "card",
            ["description"] = "Product purchase",
            ["profile_id"] = "your_profile_id", // Replace with your profile ID
            ["payment_id"] = paymentId,
            ["payment_method_data"] = new Dictionary<string, object>
            {
                ["card"] = new Dictionary<string, string>
                {
                    ["card_number"] = "4242424242424242",
                    ["card_exp_month"] = "12",
                    ["card_exp_year"] = "2025",
                    ["card_cvc"] = "123",
                    ["card_holder_name"] = "Customer Name"
                }
            }
        };
        
        var payment = await client.PostAsync<Payment>("payments", request);
        Console.WriteLine($"Payment created with status: {payment.Status}");
        
        // Step 2: Confirm the payment if needed
        if (payment.Status == "requires_confirmation")
        {
            var confirmRequest = new Dictionary<string, object>
            {
                ["payment_id"] = paymentId,
                ["payment_method"] = "card",
                ["order_details"] = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        ["product_id"] = "prod_123",
                        ["product_name"] = "Test Product",
                        ["quantity"] = 1,
                        ["amount"] = 2000
                    }
                }
            };
            
            var confirmedPayment = await client.PostAsync<Payment>($"payments/{paymentId}/confirm", confirmRequest);
            Console.WriteLine($"Payment confirmed with status: {confirmedPayment.Status}");
            
            // Step 3: Check final status
            var finalPayment = await client.Payments.RetrievePaymentAsync(paymentId);
            
            if (finalPayment.Status == "succeeded")
            {
                // Payment successful - process order
                Console.WriteLine("Payment successful!");
            }
            else
            {
                // Payment failed or pending
                Console.WriteLine($"Payment not successful. Status: {finalPayment.Status}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error processing payment: {ex.Message}");
    }
}
```

## Requirements

- .NET 6.0 SDK or later

The SDK is built to support multiple .NET versions:
- .NET 6.0
- .NET 7.0
- .NET 8.0
- .NET 9.0

## Important Note on API Parameter Naming

The HyperSwitch API uses `snake_case` for all request and response parameters (e.g., `payment_id`, `card_number`), while the .NET SDK models use `PascalCase` properties following C# conventions (e.g., `PaymentId`, `CardNumber`).

When using the SDK's strongly-typed models:
- Use C# `PascalCase` property names in your code
- The SDK automatically serializes these to `snake_case` when sending to the API
- Response data in `snake_case` is automatically deserialized to `PascalCase` properties

When using `Dictionary<string, object>` for requests:
- Always use `snake_case` keys to match the exact API parameter names
- Example: `["payment_method"]` instead of `["paymentMethod"]`

This documentation shows both approaches with comments to clarify the mapping.

## Supported Endpoints

The HyperSwitch .NET SDK provides access to the following endpoints:

### Payments API

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/payments` | `POST` | Create a new payment with specified amount, currency, and payment method |
| `/payments/{payment_id}` | `GET` | Retrieve details of a specific payment |
| `/payments/{payment_id}/confirm` | `POST` | Confirm a previously created payment |
| `/payments/{payment_id}/capture` | `POST` | Capture a previously authorized payment |
| `/payments/{payment_id}/cancel` | `POST` | Cancel a payment that hasn't been captured |


### Refunds API

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/refunds` | `POST` | Create a refund for a payment |
| `/refunds/{refund_id}` | `GET` | Retrieve details of a specific refund |
| `/refunds` | `GET` | List refunds with pagination support |

### Customers API

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/customers` | `POST` | Create a new customer |
| `/customers/{customer_id}` | `GET` | Retrieve customer details |
| `/customers/{customer_id}` | `POST` | Update customer information |
| `/customers/{customer_id}/payment_methods` | `POST` | Add a payment method to a customer |
| `/customers/{customer_id}/payment_methods` | `GET` | List customer's payment methods |

### Payment Methods API

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/payment_methods` | `GET` | Retrieve available payment methods |
| `/payment_methods/{payment_method_id}` | `GET` | Get details of a specific payment method |

## Running the Examples

The SDK includes example code that demonstrates a complete payment flow. To run the examples:

```bash
cd hyperswitch-dotnet-sdk
dotnet run --project Examples
```

### Example Payment Flow

The example demonstrates a full payment lifecycle:

1. **Creating a payment**
   - A unique payment ID is generated and passed to the API
   - The payment is created with status "requires_confirmation"

2. **Confirming the payment**
   - Using either the payment ID or client_secret to confirm the payment
   - Requires valid API credentials with appropriate permissions

3. **Retrieving the payment**
   - Getting full payment details using the payment ID

4. **Refunding the payment**
   - If payment was successful, you can issue a refund

### Important Notes for Running Examples

- **Always include a payment ID**: For consistency, use the same payment ID across all steps
- **Use snake_case parameter names**: The API expects parameters in snake_case format
- **API Key**: Replace the example API key with your actual HyperSwitch API key
- **Profile ID**: Update the profile ID to your HyperSwitch profile ID

## Usage

### Configuration

Before using the SDK, you need to configure it with your API key and optional settings:

```csharp
using HyperSwitch.SDK.Client;

// Create a configuration with API key
var config = new HyperSwitchConfiguration("your_api_key");

// Or with custom base URL (for production)
var prodConfig = new HyperSwitchConfiguration(
    apiKey: "your_api_key",
    baseUrl: "https://api.hyperswitch.io"
);
```

### Creating a Client

Once you have your configuration, you can create a client:

```csharp
using HyperSwitch.SDK.Client;

var client = new HyperSwitchClient(config);
```

### Making API Calls

#### Create Payment - Using SDK Models

```csharp
using HyperSwitch.SDK.Models;
using System;

// Using the SDK model with PascalCase properties (automatically converted to snake_case)
var paymentRequest = new CreatePaymentRequest
{
    // API parameter: id
    Id = "pay_" + Guid.NewGuid().ToString("N"),
    
    // API parameter: amount
    Amount = 1000, // $10.00 in smallest currency unit
    
    // API parameter: currency 
    Currency = "USD",
    
    // API parameter: payment_method
    PaymentMethod = "card",
    
    // API parameter: profile_id
    ProfileId = "prof_12345abcde",
    
    // API parameter: payment_method_data
    PaymentMethodData = new PaymentMethodData
    {
        // API parameter: payment_method_data.card
        Card = new CardDetails
        {
            // API parameter: payment_method_data.card.card_number
            CardNumber = "4242424242424242",
            
            // API parameter: payment_method_data.card.card_exp_month
            CardExpMonth = "12",
            
            // API parameter: payment_method_data.card.card_exp_year
            CardExpYear = "2025",
            
            // API parameter: payment_method_data.card.card_cvc
            CardCvc = "123",
            
            // API parameter: payment_method_data.card.card_holder_name
            CardHolderName = "John Doe"
        }
    },
    
    // API parameter: billing
    Billing = new ContactInformation
    {
        // API parameter: billing.address
        Address = new Address
        {
            // API parameter: billing.address.line1
            Line1 = "123 Main St",
            
            // API parameter: billing.address.city
            City = "San Francisco",
            
            // API parameter: billing.address.state
            State = "CA",
            
            // API parameter: billing.address.zip
            Zip = "94111",
            
            // API parameter: billing.address.country
            Country = "US",
            
            // API parameter: billing.address.first_name
            FirstName = "John",
            
            // API parameter: billing.address.last_name
            LastName = "Doe"
        },
        
        // API parameter: billing.phone
        Phone = new Phone
        {
            // API parameter: billing.phone.number
            Number = "8888888888",
            
            // API parameter: billing.phone.country_code
            CountryCode = "+1"
        }
    },
    
    // API parameter: description
    Description = "Test payment",
    
    // API parameter: metadata
    Metadata = new Dictionary<string, string>
    {
        { "order_id", "order_12345" },
        { "customer_id", "cust_12345" }
    }
};

var payment = await client.Payments.CreatePaymentAsync(paymentRequest);
Console.WriteLine($"Created payment with ID: {payment.Id}");
```

#### Create Payment - Using Dictionary (Alternative Approach)

```csharp
// Using Dictionary with exact snake_case keys matching the API
var paymentRequestDict = new Dictionary<string, object>
{
    ["id"] = "pay_" + Guid.NewGuid().ToString("N"),
    ["amount"] = 1000,
    ["currency"] = "USD",
    ["payment_method"] = "card",
    ["profile_id"] = "prof_12345abcde",
    ["payment_method_data"] = new Dictionary<string, object>
    {
        ["card"] = new Dictionary<string, object>
        {
            ["card_number"] = "4242424242424242",
            ["card_exp_month"] = "12",
            ["card_exp_year"] = "2025",
            ["card_cvc"] = "123"
        }
    },
    ["billing"] = new Dictionary<string, object>
    {
        ["address"] = new Dictionary<string, object>
        {
            ["line1"] = "123 Main St",
            ["city"] = "San Francisco",
            ["state"] = "CA",
            ["zip"] = "94111",
            ["country"] = "US",
            ["first_name"] = "John",
            ["last_name"] = "Doe"
        },
        ["phone"] = new Dictionary<string, object>
        {
            ["number"] = "8888888888",
            ["country_code"] = "+1"
        }
    },
    ["description"] = "Test payment",
    ["metadata"] = new Dictionary<string, string>
    {
        { "order_id", "order_12345" },
        { "customer_id", "cust_12345" }
    }
};

var paymentFromDict = await client.PostAsync<Payment>("payments", paymentRequestDict);
```

#### Retrieve a Payment

```csharp
// API path parameter: {payment_id}
var paymentId = "pay_123456789"; 
var retrievedPayment = await client.Payments.RetrievePaymentAsync(paymentId);

// Access response properties (automatically converted from snake_case)
Console.WriteLine($"Payment Status: {retrievedPayment.Status}"); // API: status
Console.WriteLine($"Payment Amount: {retrievedPayment.Amount}"); // API: amount
Console.WriteLine($"Payment Currency: {retrievedPayment.Currency}"); // API: currency
```

#### Payment Confirmation

```csharp
// API path parameter: {payment_id}
var paymentId = "pay_123456789";

// Dictionary with snake_case keys to exactly match API request parameters
var confirmRequest = new Dictionary<string, object>
{
    ["payment_method"] = "card",
    ["metadata"] = new Dictionary<string, string>
    {
        { "confirmed_by", "dotnet_sdk" },
        { "confirm_time", DateTime.UtcNow.ToString("o") }
    }
};

var confirmedPayment = await client.Payments.ConfirmPaymentAsync(paymentId, confirmRequest);
```

#### Capture Payment

```csharp
// API path parameter: {payment_id}
var paymentId = "pay_123456789";

// Dictionary with snake_case keys to exactly match API request parameters
var captureRequest = new Dictionary<string, object>
{
    ["amount_to_capture"] = 1000,
    ["statement_descriptor_suffix"] = "Order #123"
};

var capturedPayment = await client.Payments.CapturePaymentAsync(paymentId, captureRequest);
```

#### Cancel Payment

```csharp
// API path parameter: {payment_id}
var paymentId = "pay_123456789";

// Dictionary with snake_case keys to exactly match API request parameters
var cancelRequest = new Dictionary<string, object>
{
    ["cancellation_reason"] = "requested_by_customer"
};

var canceledPayment = await client.Payments.CancelPaymentAsync(paymentId, cancelRequest);
```



#### Refund a Payment

```csharp
// Dictionary with snake_case keys to exactly match API request parameters
var refundRequest = new Dictionary<string, object>
{
    ["payment_id"] = "pay_123456789",
    ["amount"] = 1000,
    ["currency"] = "USD",
    ["reason"] = "customer_requested",
    ["metadata"] = new Dictionary<string, string>
    {
        { "refunded_by", "support_agent_id_123" },
        { "refund_reason", "customer_dissatisfied" }
    }
};

// Define response class with property names matching API snake_case response
// (With JsonPropertyName attributes to handle deserialization)
public class RefundResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("refund_id")]
    public string RefundId { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("payment_id")]
    public string PaymentId { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("amount")]
    public int Amount { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("currency")]
    public string Currency { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("status")]
    public string Status { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("metadata")]
    public Dictionary<string, string> Metadata { get; set; }
}

// Call the refunds API endpoint
var refund = await client.PostAsync<RefundResponse>("refunds", refundRequest);
Console.WriteLine($"Created refund with ID: {refund.RefundId}");
```

## Error Handling

The SDK throws a `HyperSwitchApiException` when the API returns an error response. The exception includes the HTTP status code, response body, and response headers.

```csharp
try
{
    var payment = await client.Payments.CreatePaymentAsync(paymentRequest);
}
catch (HyperSwitchApiException ex)
{
    Console.WriteLine($"API error: {ex.StatusCode}");
    Console.WriteLine($"Error message: {ex.ResponseBody}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Development

### Prerequisites

- .NET SDK (6.0 or later)

### Targeting Specific .NET Versions

The SDK supports multiple .NET versions. To build for a specific version:

```bash
dotnet build -f net7.0  # Replace with desired version (net6.0, net7.0, net8.0, net9.0)
```

If you don't have a specific .NET version installed, you can either:
1. Install it from https://dotnet.microsoft.com/download
2. Build for a version you already have installed

### Running Tests

```bash
dotnet test
```

To run tests for a specific framework:
```bash
dotnet test -f net7.0
```

## Examples

See the [Examples](./Examples/) directory for sample code demonstrating how to use the SDK.

## License

This project is licensed under the MIT License - see the LICENSE file for details. 