# UserManagementAPI

A minimal ASP.NET Core Web API built for **TechHive Solutions** to manage internal user records. The project was developed across three activity phases, using **GitHub Copilot** to scaffold, debug, and enhance the codebase at each step.

---

## Tech Stack

- .NET 10 (Minimal API)
- C# 13
- In-memory `Dictionary<int, User>` (no database)
- VS Code + REST Client extension for testing

---

## Project Structure

```
UserManagementAPI/
├── Program.cs               # All API logic, middleware, and models
├── appsettings.json         # Configuration including auth token
├── requests.http            # HTTP test file for all endpoints
└── UserManagementAPI.csproj
```

---

## API Endpoints

All endpoints require a valid Bearer token in the `Authorization` header.

| Method | Route          | Description               | Success |
|--------|----------------|---------------------------|---------|
| GET    | `/users`       | Retrieve all users        | 200     |
| GET    | `/users/{id}`  | Retrieve a user by ID     | 200     |
| POST   | `/users`       | Create a new user         | 201     |
| PUT    | `/users/{id}`  | Update an existing user   | 200     |
| DELETE | `/users/{id}`  | Delete a user             | 204     |

---

## Authentication

Every request must include:

```
Authorization: Bearer techhive-secret-token-2026
```

Requests without a valid token receive `401 Unauthorized`.  
The token is configured in `appsettings.json` under `Auth:Token`.

---

## Middleware Pipeline

Middleware is registered in this order, which determines execution priority:

```
ErrorHandlingMiddleware   ← outermost: catches all unhandled exceptions
  AuthenticationMiddleware  ← validates Bearer token before any endpoint runs
    LoggingMiddleware       ← logs method, path, and status code (auth-gated)
      [Endpoints]
```

| Middleware | Behaviour |
|---|---|
| `ErrorHandlingMiddleware` | Returns `{ "error": "Internal server error." }` on unhandled exceptions |
| `AuthenticationMiddleware` | Returns `401` for missing or invalid tokens |
| `LoggingMiddleware` | Writes request/response lines to the console via `ILogger` |

---

## Input Validation

The `POST` and `PUT` endpoints validate:

- `name` — required, cannot be blank
- `email` — must match a valid email pattern
- `department` — required, cannot be blank
- Duplicate emails — rejected with `409 Conflict`

Invalid requests return `400 Bad Request` with a list of errors:

```json
{
  "errors": [
    "'name' is required and cannot be empty.",
    "'email' must be a valid email address (e.g. user@example.com)."
  ]
}
```

---

## Running the API

```bash
dotnet run
```

The API starts on `http://localhost:5098` by default.  
Use `requests.http` with the VS Code **REST Client** extension to run all tests.

---

## How GitHub Copilot Was Used

This project was built in three activities, with Copilot assisting at each stage.

### Activity 1 — Scaffolding
Copilot generated the initial `Program.cs` boilerplate and all five CRUD endpoints using a minimal API pattern. It suggested the in-memory `Dictionary<int, User>` store and the `record` types for `User` and `UserRequest`, keeping the code concise without requiring a database.

### Activity 2 — Debugging
Copilot analysed the existing code and identified several bugs:

- `UserRequest` fields were non-nullable, causing deserialization crashes on partial bodies — fixed by making them `string?` and adding a `Validate()` helper.
- No input validation meant empty names, blank departments, and malformed emails were silently stored — fixed with `IsNullOrWhiteSpace` and regex checks.
- No duplicate-email detection — fixed with a case-insensitive `Any()` check on the dictionary values.
- No `try-catch` blocks — any runtime exception produced an unformatted 500 response — fixed by wrapping each endpoint handler.
- `GET /users` returned a live `Dictionary.ValueCollection` reference — fixed by snapshotting to `.ToList()`.

### Activity 3 — Middleware
Copilot implemented the three middleware classes and configured them in the correct pipeline order:

- **Error handling first** — so no exception ever escapes the pipeline unformatted.
- **Authentication second** — so unauthenticated requests are rejected before reaching any business logic or logging.
- **Logging last** — so only authenticated, processed requests produce audit log entries.

Copilot also updated `appsettings.json` to store the token in configuration rather than hard-coding it, and rewrote `requests.http` with test cases for every middleware scenario.

---

## Seed Data

The API starts with two pre-loaded users:

| ID | Name          | Email                  | Department |
|----|---------------|------------------------|------------|
| 1  | Alice Johnson | alice@techhive.com     | HR         |
| 2  | Bob Smith     | bob@techhive.com       | IT         |
