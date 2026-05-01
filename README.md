# FinTrust Mini

Risk-aware banking transfer API with audit logging, validation, transaction safety and SQLite persistence.

FinTrust Mini is a layered .NET 8 Web API built to model a small but realistic financial backend problem: before money moves, a system should validate the request, assess transfer risk, persist the operation, keep an audit trail and protect consistency with transactions.

## Problem

Basic transfer APIs usually focus only on moving money from one account to another. In a banking context, that is not enough. A safer backend should answer these questions:

- Is the transfer request valid?
- Is the transfer risky before it is executed?
- Why was a transfer allowed or rejected?
- Can failed financial operations still be traced?
- Are account updates, transfer records and audit logs saved consistently?

FinTrust Mini addresses this through a small, explainable backend design rather than a large feature set.

## Core Features

- Account creation and account lookup
- Transfer creation and transfer lookup
- Transfer risk assessment before execution
- Rule-based risk score, risk level and decision reasons
- Request validation with standard validation responses
- Global exception handling with ProblemDetails
- Audit logging for account and transfer events
- EF Core + SQLite persistence
- Transaction-backed transfer flow
- Unit and integration tests across domain, application and infrastructure layers

## Architecture

The project follows a layered structure:

- `FinTrustMini.Domain`: business entities such as Account, Transfer and AuditLog
- `FinTrustMini.Application`: use cases, repository abstractions, risk policy and unit of work abstraction
- `FinTrustMini.Infrastructure`: EF Core SQLite persistence, repositories and transaction implementation
- `FinTrustMini.Api`: HTTP controllers, API contracts, validation and middleware
- `tests`: focused tests for domain rules, application use cases and infrastructure behavior

The API layer does not talk directly to EF Core. Controllers call application services. Application services depend on abstractions such as repositories, risk policy and unit of work. Infrastructure provides the EF Core implementations.

## Risk Assessment

`POST /api/transfers/risk-assessments` evaluates a transfer before execution.

Example response for a low-risk transfer:

```json
{
  "isAllowed": true,
  "riskLevel": "Low",
  "riskScore": 0,
  "reasons": ["No material risk signal detected."]
}
```

Example response for a limit-exceeding transfer:

```json
{
  "isAllowed": false,
  "riskLevel": "High",
  "riskScore": 100,
  "reasons": ["Transfer amount exceeds the single transfer limit of 10000."]
}
```

The same central risk policy is used by the assessment endpoint and the transfer creation use case.

## API Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/health` | Checks API health |
| `POST` | `/api/accounts` | Creates an account |
| `GET` | `/api/accounts/{accountId}` | Gets account details |
| `POST` | `/api/transfers/risk-assessments` | Evaluates transfer risk |
| `POST` | `/api/transfers` | Creates a transfer |
| `GET` | `/api/transfers/{transferId}` | Gets transfer details |

## Run Locally

Prerequisite:

- .NET 8 SDK

Restore, build and test:

```bash
dotnet restore FinTrustMini.sln
dotnet build FinTrustMini.sln
dotnet test FinTrustMini.sln
```

Run the API:

```bash
dotnet run --project src/FinTrustMini.Api/FinTrustMini.Api.csproj
```

Development settings use SQLite:

```text
Data Source=fintrust-mini-dev.db
```

The project currently uses `EnsureCreated` for a lightweight local setup. In a production-grade version, EF Core migrations would be used to version database schema changes.

## Example Requests

Create an account:

```bash
curl -X POST http://localhost:5050/api/accounts \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": "11111111-1111-1111-1111-111111111111",
    "iban": "TR000000000000000000000001",
    "openingBalance": 1000
  }'
```

Assess transfer risk:

```bash
curl -X POST http://localhost:5050/api/transfers/risk-assessments \
  -H "Content-Type: application/json" \
  -d '{
    "fromAccountId": "11111111-1111-1111-1111-111111111111",
    "toAccountId": "22222222-2222-2222-2222-222222222222",
    "amount": 250,
    "description": "Invoice payment"
  }'
```

## Testing

The test suite covers:

- Domain rules for accounts and transfers
- Application use cases for account and transfer workflows
- Risk assessment behavior
- EF Core repository persistence
- Unit of work commit and rollback behavior

Run:

```bash
dotnet test FinTrustMini.sln
```

## Technical Decisions

- **Layered architecture:** Keeps HTTP, use case, domain and persistence responsibilities separate.
- **Repository abstractions:** Application code is not tied to EF Core or SQLite.
- **Central risk policy:** Risk decisions are not duplicated between endpoints.
- **ProblemDetails:** API errors use a standard response shape.
- **Audit log:** Important financial operations leave an operational trace.
- **Transaction unit of work:** Transfer-related writes are handled atomically.
- **SQLite:** Keeps local setup simple while still using real persistence.

## Interview Pitch

FinTrust Mini is a risk-aware banking transfer API. I built it to model the backend concerns behind financial transfers: validating incoming requests, assessing risk before execution, recording audit logs, persisting data with EF Core and protecting multi-step transfer operations with transactions. The project is intentionally small, but it demonstrates how I think about layered architecture, business rules, consistency and traceability in a financial system.
