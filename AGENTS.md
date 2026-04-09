# Agent Notes

## Build & Run

```bash
dotnet build
dotnet run
```

Dev server: `http://localhost:5188`. Docs: `/docs`.

## Architecture

- **.NET 10.0** Minimal APIs
- **CQRS** with Read/Write separation
- **MediatR** for Commands/Queries
- **FluentValidation** for validation
- **FluentResults** for error handling
- **EF Core + PostgreSQL** (WriteDbContext/ReadDbContext)

## Database Schema (3NF)

```
users ──────┬────── projects
  │         │        │
  │         │ 1:N   │
  │         └────────┤
  │                  │ 1:N
  └──────────────────┴──── tasks
            (assignee)
```

### Tables
- **users**: id, email, display_name, timestamps
- **projects**: id, name, description, owner_id (FK→users), timestamps
- **tasks**: id, title, description, is_completed, project_id (FK→projects), assignee_id (FK→users, nullable), created_by (FK→users), timestamps

## Features

```
Features/
├── Users/     # CRUD users
├── Projects/  # CRUD projects
└── Tasks/     # Nested under projects
```

## Key Patterns

### Feature Structure
```
Features/{Name}/
├── Domain/           # Entity, IRepository, Errors
├── Application/      # Commands, Queries, Validators
├── Infrastructure/   # Repository implementation
├── Endpoint/         # API endpoints
└── {Name}Module.cs  # Service registration
```

### Response Format
- Success: `ResponseWithMetadata<T>` wrapper
- Error: RFC 7807 ProblemDetails

## Database Migrations

```bash
dotnet ef migrations add <Name> --context WriteDbContext
dotnet ef database update --context WriteDbContext
```

## API Testing

Use `isc-tmr-backend.http` for REST testing.
