# Payment Gateway Design and Implementation

## Design

The ASP.NET Core 3.1 Web API application containing a REST API that allows payments to be processed and historical payments 
to be retrieved.

The Open API (Swagger) specification of the REST API can be found [here](https://patituccia.github.io/payment-gateway).

My main reference for .Net Core microservices is the [.NET Microservices: Architecture for Containerized .NET Applications](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/).
An application layer defines the contract on how to access the microservice (Controller, REST API). A domain layer contains the business 
logic, entities and domain events. An infrastructure layer that takes care of persistence or dealing with external systems.

It is assumed that the microservice should be able to serve requests for any registered merchant and process the request with the 
corresponding acquiring bank.

### Controllers

[```PaymentsController```](/PaymentGateway/Controllers/PaymentController.cs) have a POST method to process new payments 
and a GET method to find previously processed payments.

All the external communication is done through the [```DTO```](/PaymentGateway/Models) models. The models use data annotations
to speficy required values and data validation, enforced by the ASP.NET Core framework (e.g. regular expressions or Credit Card).

A Controller can also be created to create and view existing merchants.

### Domain

The Domain Model contains two main entities [```Merchant```](/PaymentGateway.Domain/Merchant.cs) and [```Payment```](/PaymentGateway.Domain/Payment.cs)
which are persisted. The [```PaymentRequest```](/PaymentGateway.Domain/PaymentRequest.cs) and [```PaymentResponse```](/PaymentGateway.Domain/PaymentResponse.cs) 
entities are transient (value objects) and used to fulfill the payment transaction.

An interface is defined [```IAcquiringBank```](/PaymentGateway.Domain/IAcquiringBank.cs) that should be implemented by a provider to
fulfill the payment request. The implementer should be able to resolve the concrete acquiring bank service for a particular merchant and 
carry out the transaction.

Instead of defining specific interfaces for saving and finding entities (e.g. repositories) I've created (request/response) domain events:
[```SaveMerchant```](/PaymentGateway.Domain/Events/SaveMerchant.cs), [```FindMerchant```](/PaymentGateway.Domain/Events/FindMerchant.cs),
[```SavePayment```](/PaymentGateway.Domain/Events/SavePayment.cs), and [```FindPayment```](/PaymentGateway.Domain/Events/FindPayment.cs).
The idea is to reduce coupling with the system that will actually carry out these requests. I used an API called [MediatR](https://github.com/jbogard/MediatR/wiki)
which is referenced in the [Domain events: design and implementation](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation)
section of the previously mentioned book.

### Tests

I've implemented Unit Tests for the [Controllers](/PaymentGateway.Tests) and [Domain](/PaymentGateway.Domain.Tests) classes 
using [xUnit](https://xunit.net/), [NSubstitute](https://nsubstitute.github.io/) for mocking, and in the Domain tests I used [Fluent 
Assertions](https://fluentassertions.com/).

There are no controller integration tests which can be achieved by using a [TestServer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.testhost.testserver) 
using the approach [defined here](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1).

## Implemented features

### Logging

I've used [Serilog](https://serilog.net/) for logging instead of the default logging because it supports [message templates](https://messagetemplates.org/)
which improves machine-readability of log messages.

### Containerization

### Build Script / CI

### Data Storage

## Non-implemented features

### Metrics

### Authentication

### API Client

### Performance testing

### Encryption
