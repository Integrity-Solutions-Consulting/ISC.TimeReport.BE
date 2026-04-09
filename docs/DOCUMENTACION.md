# Documentación Técnica - ISC TMR Backend

Bienvenido al proyecto ISC TMR Backend. Este documento te guiará a través de la arquitectura, patrones y buenas prácticas utilizadas en el proyecto.

## 📚 Tabla de Contenidos

1. [Arquitectura General](#arquitectura-general)
2. [Modelo de Datos](#modelo-de-datos)
3. [Patrones y Convenciones](#patrones-y-convenciones)
4. [Guía para Crear una Nueva Feature](#guía-para-crear-una-nueva-feature)
5. [Estructura de Respuestas API](#estructura-de-respuestas-api)
6. [Base de Datos y Migraciones](#base-de-datos-y-migraciones)

---

## Arquitectura General

### Stack Tecnológico

| Tecnología | Propósito |
|------------|-----------|
| **.NET 10.0** | Framework principal |
| **Minimal APIs** | Endpoints HTTP ligera y moderna |
| **MediatR** | Implementación del patrón CQRS |
| **FluentResults** | Manejo de errores tipados |
| **FluentValidation** | Validación de requests |
| **EF Core + PostgreSQL** | Acceso a datos |
| **CQRS** | Separación de Commands y Queries |

### Arquitectura CQRS

El proyecto implementa **CQRS (Command Query Responsibility Segregation)**:

```
┌─────────────────────────────────────────────────────────────────────┐
│                           API Layer                                   │
│   POST/PUT/DELETE                              GET                    │
│         │                                         │                 │
│         ▼                                         ▼                 │
│   ┌─────────────┐                          ┌─────────────┐         │
│   │  Commands   │                          │   Queries    │         │
│   └──────┬──────┘                          └──────┬──────┘         │
└───────────┼─────────────────────────────────────┼─────────────────┘
            │                                     │
            ▼                                     ▼
┌─────────────────────────┐        ┌─────────────────────────┐
│    WriteDbContext        │        │    ReadDbContext         │
│    (PostgreSQL)          │        │    (PostgreSQL/Redis)    │
│    - Tracking enabled     │        │    - NoTracking          │
└─────────────────────────┘        └─────────────────────────┘
```

---

## Modelo de Datos (3FN)

### Diagrama de Relaciones

```
┌─────────────────────────────────────────────────────────────────────────┐
│                              USERS                                        │
│  ┌─────────────┐                                                        │
│  │     id      │  (PK - viene del Entra ID)                           │
│  │    email    │                                                        │
│  │ display_name│                                                        │
│  └─────────────┘                                                        │
│         │                                                               │
│         │ 1:N                                                           │
│         ▼                                                               │
│  ┌─────────────────┐         ┌─────────────────┐                         │
│  │    PROJECTS     │         │     TASKS       │                         │
│  │ ─────────────── │         │ ─────────────── │                         │
│  │ id (PK)        │         │ id (PK)         │                         │
│  │ name           │         │ title           │                         │
│  │ description    │         │ description     │                         │
│  │ owner_id (FK)──┼──1:N───►│ project_id (FK) │◄──────┐               │
│  └────────────────┘         │ assignee_id (FK)│       │               │
│                               │ created_by (FK) │       │               │
│                               │ is_completed    │       │               │
│                               └─────────────────┘       │               │
│                                                       │               │
└───────────────────────────────────────────────────────┼───────────────┘
                                                        │ 1:N
                                                        │ (created_by)
                                                        ▼
                                                 users (mismo)
```

### Tablas

#### USERS
| Campo | Tipo | Descripción |
|-------|------|-------------|
| id | UUID | PK |
| email | VARCHAR(255) | Email |
| display_name | VARCHAR(100) | Nombre para mostrar |
| created_at | TIMESTAMPTZ | |
| updated_at | TIMESTAMPTZ | |

#### PROJECTS
| Campo | Tipo | Descripción |
|-------|------|-------------|
| id | UUID | PK |
| name | VARCHAR(200) | Nombre |
| description | TEXT | Descripción |
| owner_id | UUID | FK → users.id |
| created_at | TIMESTAMPTZ | |
| updated_at | TIMESTAMPTZ | |

#### TASKS
| Campo | Tipo | Descripción |
|-------|------|-------------|
| id | UUID | PK |
| title | VARCHAR(200) | Título |
| description | TEXT | Descripción |
| is_completed | BOOLEAN | Estado |
| project_id | UUID | FK → projects.id |
| assignee_id | UUID | FK → users.id (nullable) |
| created_by | UUID | FK → users.id |
| created_at | TIMESTAMPTZ | |
| updated_at | TIMESTAMPTZ | |

---

## Patrones y Convenciones

### 1. Estructura de una Feature

```
Features/{Name}/
├── Domain/
│   ├── {Name}.cs
│   ├── I{Name}Repository.cs
│   └── {Name}Errors.cs
├── Application/
│   ├── Commands/
│   │   ├── Create{Name}Command.cs
│   │   ├── Create{Name}CommandHandler.cs
│   │   └── ...
│   ├── Queries/
│   │   └── ...
│   └── Validators/
│       └── ...
├── Infrastructure/
│   └── Repositories/
│       └── {Names}Repository.cs
├── Endpoint/
│   └── {Name}Endpoint.cs
└── {Name}Module.cs
```

### 2. Patrón de Entidad

```csharp
public class Entity
{
    public Guid Id { get; private set; }
    // ... otros campos
    
    private Entity() { }
    
    public static Entity Create(...) { }
    public void Update(...) { }
}
```

### 3. Repositorio

```csharp
public interface IEntityRepository
{
    Task<Result<Entity>> AddAsync(Entity entity);
    Task<Result<Entity>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<Entity>>> GetAllAsync();
    Task<Result<Entity>> UpdateAsync(Entity entity);
    Task<Result> DeleteAsync(Guid id);
}
```

---

## Guía para Crear una Nueva Feature

### Paso 1: Crear estructura de carpetas
```bash
mkdir -p Features/Entity/Domain
mkdir -p Features/Entity/Application/{Commands,Queries,Validators}
mkdir -p Features/Entity/Infrastructure/Repositories
mkdir -p Features/Entity/Endpoint
```

### Paso 2: Implementar Domain
- Entidad con factory method
- Interfaz del repositorio
- Errores tipados

### Paso 3: Implementar Infrastructure
- Repositorio usando `WriteDbContext`

### Paso 4: Implementar Application
- Commands y Handlers
- Queries y Handlers
- Validators

### Paso 5: Implementar Endpoint
```csharp
public static class EntityEndpoint
{
    public static RouteGroupBuilder MapEntityEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/entities", GetAll)
            .Produces<ResponseWithMetadata<IEnumerable<GetEntityResponse>>>(200);
        // ...
    }
}
```

### Paso 6: Registrar en Module
```csharp
public static IServiceCollection AddEntityServices(this IServiceCollection services)
{
    services.AddScoped<IEntityRepository, EntitiesRepository>();
    return services;
}
```

### Paso 7: Actualizar extensiones
```csharp
// ServiceCollectionExtensions.cs
services.AddEntityServices();

// WebApplicationExtensions.cs
app.MapEntityEndpoints();
```

### Paso 8: Crear migración
```bash
dotnet ef migrations add AddEntity --context WriteDbContext
dotnet ef database update --context WriteDbContext
```

---

## Estructura de Respuestas API

### Respuesta Exitosa
```json
{
  "data": { ... },
  "metadata": {
    "message": "Operation successful",
    "status": 200,
    "pagination": null
  }
}
```

### Respuesta de Error (RFC 7807)
```json
{
  "type": "https://tools.ietf.org/html/rfc7807",
  "title": "Bad Request",
  "status": 400,
  "detail": "Error message",
  "instance": "POST /api/...",
  "traceId": "...",
  "timestamp": "..."
}
```

---

## Base de Datos y Migraciones

### Conexiones
```json
{
  "ConnectionStrings": {
    "WriteDbConnection": "postgresql://...",
    "ReadDbConnection": "postgresql://..."
  }
}
```

### Comandos
```bash
dotnet ef migrations add Nombre --context WriteDbContext
dotnet ef database update --context WriteDbContext
```

### Relaciones
- **projects.owner_id** → users.id (RESTRICT)
- **tasks.project_id** → projects.id (CASCADE)
- **tasks.assignee_id** → users.id (SET NULL)
- **tasks.created_by** → users.id (RESTRICT)
