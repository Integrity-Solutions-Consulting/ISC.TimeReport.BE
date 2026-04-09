# Agent Notes

## Build & Run

```bash
dotnet build
dotnet run
```

Dev server runs on `http://localhost:5188`. API docs (Scalar) at `/docs`.

## Architecture

- **.NET 10.0** ASP.NET Core Web API with Minimal APIs (endpoint routing)
- **MediatR** for CQRS (Commands/Queries pattern in `Application/` folders)
- **FluentValidation** for request validation
- **FluentResults** for error handling (not exceptions)
- **EF Core + PostgreSQL** via `AppDbContext` in `Infrastructure/Persistence/`
- **API versioning** via `X-Api-Version` header (default v1.0)
- **API prefix**: `/api` (configured in `appsettings.json` `Server:Prefix`)

## Directory Layout

```
Program.cs                      # Entry point; wires up extensions
Infrastructure/
  Extensions/                   # Service registration (Add* methods)
  Persistence/                   # DbContext, entity configs
Features/
  Notifications/                # Feature slice (CQRS: Commands, Queries, Domain, Infrastructure)
  Todo/                         # Minimal API feature (uses Controllers)
```

## Key Patterns

### Adding a Feature
1. Create `Features/{Name}/` folder
2. Add `Add{Name}Services()` extension method in `Infrastructure/Extensions/`
3. Add `Map{Name}Endpoints()` extension method in `Infrastructure/Extensions/WebApplicationExtensions.cs`
4. Register services in `Program.cs`

### Endpoints return `Result<T>` (FluentResults)
- `Result.IsSuccess` → HTTP success (201/200)
- `Result.IsFailed` → 400 Bad Request with error messages

### Validation
- Validators inherit from `FluentValidation.AbstractValidator<T>`
- Endpoints call `validator.ValidateAsync()` before MediatR dispatch

## Known Issues

- `WebApplicationExtensions.cs` references `ActivitiesModule` but it does not exist (commented out references may be needed)
- `DatabaseExtensions.cs` registers two `AppDbContext` instances (both PostgreSQL and DefaultConnection) — likely unintentional duplication

## EF Core Migrations

```bash
dotnet tool install --global dotnet-ef  # one-time
dotnet ef migrations add <Name>
dotnet ef database update
```

## API Testing

Use `isc-tmr-backend.http` for REST testing (VS Code REST Client extension).

## Environment

- `ASPNETCORE_ENVIRONMENT=Development` enables sensitive data logging and detailed errors
- Connection strings in `appsettings.json` (use `.env` for secrets — already gitignored)
