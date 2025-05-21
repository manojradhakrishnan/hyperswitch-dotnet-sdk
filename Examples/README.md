# Testing the HyperSwitch .NET SDK

This folder contains example code to test the HyperSwitch .NET SDK functionality.

## Prerequisites

- .NET SDK (6.0 or later) installed on your machine
- An API key from HyperSwitch (can be obtained from your HyperSwitch dashboard)

## Running the Example

1. Update the API key in the `PaymentExample.cs` file:
   ```csharp
   var apiKey = "your_api_key"; // Replace with your actual API key
   ```

2. Build and run the example:
   ```bash
   cd hyperswitch-dotnet-sdk
   dotnet build
   dotnet run --project Examples
   ```

## Selecting a Specific .NET Target Framework

The SDK supports multiple .NET versions (6.0, 7.0, 8.0, and 9.0). By default, it will use the latest version installed on your machine, but you can specify which version to use:

### To build for a specific framework:

```bash
dotnet build -f net7.0  # Replace with your preferred version (net6.0, net7.0, net8.0, net9.0)
```

### To run with a specific framework:

```bash
dotnet run --project Examples -f net7.0  # Replace with your preferred version
```

If you encounter framework-related errors, you can either:
1. Install the required .NET SDK version from https://dotnet.microsoft.com/download
2. Specify a different target framework that's already installed on your machine

## What the Example Does

The example demonstrates:

1. Creating a payment using a test card
2. Retrieving the created payment
3. Listing recent payments

## Expected Output

If everything works correctly, you should see output similar to:

```
Creating a payment...
Created payment with ID: pay_xyz123
Amount: 20.0 USD
Status: requires_confirmation

Retrieving payment with ID: pay_xyz123
Retrieved payment status: requires_confirmation

Listing recent payments:
Found 5 payments in total
- Payment ID: pay_xyz123, Amount: 20.0 USD, Status: requires_confirmation
- Payment ID: pay_abc456, Amount: 15.0 USD, Status: succeeded
- Payment ID: pay_def789, Amount: 10.0 USD, Status: succeeded
...
```

## Troubleshooting

If you encounter any errors:

- Verify that your API key is correct
- Check that you're using the correct base URL (sandbox for testing, production for live)
- Ensure your HyperSwitch account is properly configured
- Make sure you're using a .NET version that's installed on your machine (use `-f` flag to specify)

If you need further assistance, please refer to the [HyperSwitch API documentation](https://hyperswitch.io/docs) or contact HyperSwitch support. 