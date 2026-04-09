# Documentación ISC TMR Backend

## 📚 Contenido

- **[Quick Start](QUICKSTART.md)** - Primeros pasos para nuevos desarrolladores
- **[Documentación Completa](DOCUMENTACION.md)** - Arquitectura, patrones y guías detalladas
- **[Glosario](../AGENTS.md)** - Términos técnicos

## 🚀 Inicio Rápido

```bash
dotnet restore
dotnet tool restore
dotnet ef database update --context WriteDbContext
dotnet run
```

API: `http://localhost:5188`
Docs: `http://localhost:5188/docs`

## 🏗️ Arquitectura

- **CQRS** - Commands y Queries separados
- **MediatR** - Desacoplamiento
- **FluentValidation** - Validación
- **FluentResults** - Errores tipados
- **EF Core + PostgreSQL** - Datos
- **Read/Write DbContexts** - Preparado para Redis

## 📊 Modelo de Datos

```
users ──────┬────── projects
  │         │        │
  │         │        │ 1:N
  │         │        └──────┐
  │         │               │
  │         │               ▼
  │         │           tasks
  │         │               ▲
  └─────────┼───────────────┘
            │ (assignee)
```

## 📡 Features

| Feature | Descripción |
|---------|-------------|
| **Users** | CRUD de usuarios |
| **Projects** | Proyectos (pertenecen a users) |
| **Tasks** | Tareas (pertenecen a projects) |

Ver [Quick Start](QUICKSTART.md) para más detalles.
