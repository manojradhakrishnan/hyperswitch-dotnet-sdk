# HyperSwitch .NET SDK - Technical Context

## Technologies Used

### Core Framework
- **.NET Standard 2.0** - Base target for maximum compatibility
- **Multi-targeting** - Supports .NET 6.0, 7.0, 8.0, and 9.0

### Dependencies
- **Newtonsoft.Json** - Primary JSON serialization/deserialization library
- **System.Text.Json** - Secondary JSON handling for modern .NET versions
- **System.Net.Http** - HTTP client for API communication

### Development Tools
- **Visual Studio / VS Code** - Primary development environments
- **NuGet** - Package management
- **MSBuild** - Build system
- **xUnit** - Testing framework

## Development Setup

### Project Structure
- **src/HyperSwitch.SDK/** - Main SDK project
  - **Api/** - API endpoint implementations
  - **Client/** - Client and configuration classes
  - **Models/** - Data models for requests and responses
- **tests/** - Unit and integration tests
- **Examples/** - Example applications showing SDK usage
- **HyperSwitch.SDK.sln** - Solution file

### Build Configuration
- **Target Frameworks**: net6.0;net7.0;net8.0;net9.0
- **Package Configuration**: NuGet package metadata in .csproj file
- **XML Documentation**: Generated from code comments for IntelliSense support

## Technical Constraints

### Backward Compatibility
- Must maintain compatibility with existing .NET ecosystem
- Should support older .NET versions where possible
- API design must follow consistent patterns for all endpoints

### Performance Considerations
- Minimize memory allocations in hot paths
- Use `ConfigureAwait(false)` for async methods
- Proper `HttpClient` lifetime management
- Efficient serialization/deserialization

### Security Requirements
- No storage of API keys in plaintext
- Secure handling of payment information
- HTTPS enforcement for all API communication
- Proper error handling without exposing sensitive information

## Dependencies

### External Services
- **HyperSwitch API** - Payment processing platform
  - Base URL: https://sandbox.hyperswitch.io (sandbox)
  - Base URL: https://api.hyperswitch.io (production)
  - Authentication: API key in request headers

### Required Environment Configuration
- **API Key** - Required for authentication
- **Base URL** - Configurable for sandbox/production environments
- **Timeout Settings** - Configurable request timeouts

## Testing Strategy
- **Unit Tests** - Testing individual components in isolation
- **Integration Tests** - Testing against HyperSwitch sandbox environment
- **Mocking** - HttpClient mocking for unit tests
- **Test Coverage** - Aim for high test coverage on core functionality

## Deployment Process
- **NuGet Package** - Primary distribution method
- **Versioning** - Semantic versioning (MAJOR.MINOR.PATCH)
- **Release Notes** - Documented changes for each version
- **CI/CD** - Automated builds and tests on code changes 