# HyperSwitch .NET SDK - Project Brief

## Project Overview
The HyperSwitch .NET SDK is an official client library that provides a convenient interface to the HyperSwitch Payments API. It enables .NET developers to integrate HyperSwitch payment processing capabilities into their applications.

## Core Requirements

### Functionality
- Provide a simple, consistent interface to all HyperSwitch API endpoints
- Support typesafe request and response models
- Implement comprehensive error handling
- Maintain compatibility with multiple .NET versions (6.0, 7.0, 8.0, 9.0)
- Enable asynchronous operations through async/await support

### Technical Requirements
- Maintain clean separation of concerns (API, Client, Models)
- Implement proper serialization/deserialization for snake_case API parameters to PascalCase .NET properties
- Provide robust error handling and logging capabilities
- Support authentication via API keys
- Implement proper HTTP request/response handling

### User Experience
- Offer a straightforward, idiomatic C# interface following .NET conventions
- Support both strongly-typed models and dynamic dictionary-based requests
- Provide detailed documentation and examples
- Minimize dependencies on external libraries

## Project Scope
The SDK covers the following HyperSwitch API areas:
- Payments API (create, retrieve, confirm, capture, cancel)
- Refunds API
- Customers API
- Payment Methods API

## Success Criteria
1. Developers can perform complete payment flows with minimal code
2. The SDK handles all serialization/deserialization and authentication transparently
3. Errors are properly caught and exposed with meaningful context
4. Documentation and examples cover all major use cases 