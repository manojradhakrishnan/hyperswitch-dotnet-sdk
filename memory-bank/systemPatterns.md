# HyperSwitch .NET SDK - System Patterns

## System Architecture
The HyperSwitch .NET SDK follows a clean, layered architecture:

1. **Client Layer** (`HyperSwitchClient.cs`)
   - Core HTTP client handling authentication and request/response processing
   - Manages API key authentication and base URL configuration
   - Provides low-level HTTP methods (GET, POST, PUT)
   - Handles request/response logging
   - Manages exception handling and error responses

2. **API Layer** (`Api/*.cs`)
   - Provides domain-specific API interfaces (Payments, Refunds, Customers)
   - Maps .NET method calls to appropriate HTTP endpoints
   - Uses the client layer for communication
   - Offers strongly-typed methods for common operations

3. **Model Layer** (`Models/*.cs`)
   - Contains request/response data models
   - Handles serialization/deserialization between C# and JSON
   - Manages property naming conventions (PascalCase in C#, snake_case in API)

## Key Technical Decisions

### HTTP Client Management
- Uses `HttpClient` for all API communication
- Supports custom `HttpClient` injection for advanced scenarios
- Implements proper lifetime management with IDisposable pattern

### Serialization Strategy
- Uses Newtonsoft.Json for serialization/deserialization
- Handles snake_case to PascalCase conversion transparently
- Ignores null values when serializing to reduce payload size

### Error Handling
- Custom `HyperSwitchApiException` class for API errors
- Includes status code, response body, and headers in exceptions
- Provides detailed logging capabilities for troubleshooting

### Interface Design
- Fluent interface with method chaining where appropriate
- Consistent naming patterns for all API methods
- Strong emphasis on async/await pattern for all operations

## Design Patterns in Use

### Factory Pattern
- `HyperSwitchClient` acts as a factory for API endpoints (Payments, Refunds, etc.)
- Each API instance is created and configured by the client

### Dependency Injection
- Constructor injection for dependencies
- Allows for custom `HttpClient` and configuration injection
- Supports testability through interface-based design

### Builder Pattern
- Complex request objects use builder-like patterns for construction
- Fluent interfaces for model creation

### Observer Pattern
- Event-based notification system for request/response monitoring
- `RequestSent` and `ResponseReceived` events for external monitoring

## Component Relationships

```
HyperSwitchConfiguration ─┐
                          │
                          ▼
                    HyperSwitchClient ───┬───► PaymentsApi
                          │              │
                          │              ├───► RefundsApi
                          │              │
                          │              └───► CustomersApi
                          ▼
               HTTP Request/Response ────► HyperSwitch API
```

## Code Style and Conventions
- XML documentation on all public classes and methods
- Consistent naming conventions following Microsoft guidelines
- Comprehensive exception handling
- Async methods with ConfigureAwait(false) for performance
- Strong typing where possible, with dictionary fallback for flexibility 