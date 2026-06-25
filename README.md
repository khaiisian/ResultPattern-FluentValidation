# ResultPattern + FluentValidation

A small ASP.NET Core Web API (.NET 10) built as a **learning project** to combine two
patterns that are commonly used together in modern .NET APIs:

- **Result Pattern** — operations return a `Result<T>` object describing the *outcome*
  (success, not found, failure, validation error) instead of throwing exceptions or
  returning bare data. The controller translates that outcome into the right HTTP status code.
- **FluentValidation** — request input is validated with declarative rules, and validation
  failures are funnelled **into the same `Result<T>` envelope** so the API returns one
  consistent response shape for everything.

The domain is a simple **Item** CRUD (`Id`, `Name`, `Qty`, `Price`).

---

## Why combine them?

They solve two different problems that sit side by side on every request:

| Pattern | Answers |
|---|---|
| FluentValidation | *"Is the incoming input well-formed?"* |
| Result Pattern | *"What was the outcome of the operation?"* |

By routing validation errors through the Result envelope, **every** response — validation
errors, not-found, business failures, and success — has the same JSON shape.

---

## Architecture

The solution is split into four projects with a one-directional reference chain (no cycles):

```
Api          → controllers, validators, validation filter         (refs Services)
Services     → IItemService/ItemService, Result<T>, request/response models   (refs DataAccess, Db)
DataAccess   → IItemRepository/ItemRepository (raw EF Core access) (refs Db)
Db           → AppDbContext + TblItem entity                       (base)

Reference chain:  Api → Services → DataAccess → Db
```

### Responsibility per layer

| Layer | Project | Responsibility |
|---|---|---|
| **Db** | `ResultPattern-FluentValidation.Db` | `AppDbContext` and the `TblItem` entity (EF Core, SQL Server). |
| **DataAccess** | `ResultPattern-FluentValidation.DataAccess` | Pure data access. Returns raw entities (`TblItem`, `List<TblItem>`, row counts). Knows nothing about `Result`. |
| **Services** | `ResultPattern-FluentValidation.Services` | Business logic. Calls the repository, decides the outcome, **formats `Result<T>`**, and maps entities → response models. |
| **Api** | `ResultPattern-FluentValidation.Api` | Controllers map `Result.Type` → HTTP status. Validation runs in a global action filter before the action executes. |

---

## What I implemented

1. **`Result<T>` type** (`Services/Shared/Result.cs`)
   - Carries `IsSuccess`, `Data`, `Message`, `Errors` (list), and a `ResultType` enum
     (`Success`, `BadRequest`, `NotFound`, `Failure`, `ValidationError`, `SystemError`).
   - Built only through static factory methods (`Success`, `NotFound`, `Failure`,
     `ValidationError`) — the constructor is private.

2. **Repository layer** (`DataAccess`)
   - `IItemRepository` / `ItemRepository` — `GetAll`, `GetById`, `Add`, `Update`, `Delete`.
   - Returns raw entities; no business logic.

3. **Service layer** (`Services`)
   - `IItemService` / `ItemService` — wraps repository calls in `Result<T>`, applies the
     not-found / failure logic, and maps `TblItem` to response models.
   - Request models: `CreateItemRequest`, `UpdateItemRequest`.
   - Response models: `ItemResponse` (reads), `CreateItemResponse`, `UpdateItemResponse`.
   - The database entity (`TblItem`) never leaves this layer.

4. **FluentValidation**
   - `CreateItemRequestValidator`, `UpdateItemRequestValidator` define the input rules.
   - The update validator uses `.When(...)` so optional PATCH fields are only validated
     when supplied.

5. **Validation → Result integration** (`Api/Filters/ValidationFilter.cs`)
   - A global `IAsyncActionFilter` resolves the matching `IValidator<T>` for each incoming
     model, validates it, and on failure short-circuits with
     `Result<object>.ValidationError(errors)` (HTTP 400) — before the controller runs.
   - The built-in `[ApiController]` auto-400 is suppressed (`SuppressModelStateInvalidFilter`)
     and FluentValidation's auto-validation is disabled so the filter is the single source
     of validation responses.

6. **Controller** (`Api/Controllers/ItemController.cs`)
   - Thin: calls the service and maps `Result.Type` to `Ok` / `NotFound` / `500`.

---

## Request flow

```
HTTP request
  → ValidationFilter (FluentValidation)
        invalid → Result.ValidationError → 400  (stops here)
        valid   ↓
  → ItemController (action)
  → IItemService (business logic, builds Result<T>, maps to response model)
  → IItemRepository (EF Core data access)
  → AppDbContext / SQL Server
  ← Result<T> bubbles back up
  ← Controller maps Result.Type → HTTP status code
```

## Example responses

**Validation error (400):**
```json
{
  "isSuccess": false,
  "data": null,
  "message": "Validation Failed.",
  "errors": ["'Name' must not be empty.", "'Price' must be greater than or equal to '0'."],
  "type": 4
}
```

**Success (200):**
```json
{
  "isSuccess": true,
  "data": { "id": 1, "name": "Pen", "qty": 10, "price": 2.50 },
  "message": "Save successful.",
  "errors": [],
  "type": 0
}
```

---

## Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/item` | Get all items |
| GET | `/api/item/{id}` | Get item by id |
| POST | `/api/item` | Create an item |
| PATCH | `/api/item/{id}` | Partially update an item |
| DELETE | `/api/item/{id}` | Delete an item |

---

## Running locally

1. Set the `DefaultConnection` connection string in
   `ResultPattern-FluentValidation.Api/appsettings.json` to your SQL Server instance.
2. Build and run:
   ```bash
   dotnet build
   dotnet run --project ResultPattern-FluentValidation.Api
   ```
3. Open the root URL — Swagger UI is served there in Development.

---

## Tech stack

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core 10 (SQL Server)
- FluentValidation 12
- Swagger / Swashbuckle
