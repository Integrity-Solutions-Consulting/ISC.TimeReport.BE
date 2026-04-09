# Quick Start - Guía Rápida para Desarrolladores

¿Nuevo en el proyecto? Esta guía te ayudará a comenzar rápidamente.

## 🚀 Primeros Pasos

### 1. Requisitos
- .NET 10.0 SDK
- PostgreSQL
- Visual Studio Code o Visual Studio

### 2. Configuración Inicial
```bash
git clone <repo-url>
cd isc-tmr-backend
dotnet restore
dotnet tool restore
```

### 3. Configurar Base de Datos
Edita `appsettings.json`:
```json
"ConnectionStrings": {
  "WriteDbConnection": "Host=...;Database=...;Username=...;Password=...",
  "ReadDbConnection": "Host=...;Database=...;Username=...;Password=..."
}
```

### 4. Ejecutar Migraciones
```bash
dotnet ef database update --context WriteDbContext
```

### 5. Iniciar el Servidor
```bash
dotnet run
```
Disponible en: `http://localhost:5188`

---

## 📁 Estructura del Proyecto

```
Features/
├── Users/         # CRUD de usuarios
├── Projects/      # Proyectos
└── Tasks/         # Tareas (anidadas bajo proyectos)
```

---

## 🔧 Comandos Útiles

| Comando | Descripción |
|---------|-------------|
| `dotnet build` | Compilar |
| `dotnet run` | Ejecutar |
| `dotnet ef migrations add <name>` | Crear migración |
| `dotnet ef database update` | Aplicar migraciones |

---

## 📡 Endpoints

### Users
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/users` | Listar usuarios |
| GET | `/api/users/{id}` | Obtener usuario |
| POST | `/api/users` | Crear usuario |
| PUT | `/api/users/{id}` | Actualizar usuario |
| DELETE | `/api/users/{id}` | Eliminar usuario |

### Projects
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/projects` | Listar proyectos |
| GET | `/api/projects/{id}` | Obtener proyecto |
| GET | `/api/projects/owner/{ownerId}` | Proyectos del owner |
| POST | `/api/projects` | Crear proyecto |
| PUT | `/api/projects/{id}` | Actualizar proyecto |
| DELETE | `/api/projects/{id}` | Eliminar proyecto |

### Tasks (anidadas bajo Projects)
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/projects/{id}/tasks` | Tareas del proyecto |
| GET | `/api/projects/{id}/tasks/{taskId}` | Obtener tarea |
| GET | `/api/tasks/assignee/{assigneeId}` | Tareas asignadas |
| POST | `/api/projects/{id}/tasks` | Crear tarea |
| PUT | `/api/projects/{id}/tasks/{taskId}` | Actualizar tarea |
| PATCH | `/api/projects/{id}/tasks/{taskId}/complete` | Completar tarea |
| DELETE | `/api/projects/{id}/tasks/{taskId}` | Eliminar tarea |

---

## ❓ Preguntas Frecuentes

**Q: ¿Por qué Tasks están anidadas bajo Projects?**
R: Las tareas siempre pertenecen a un proyecto específico.

**Q: ¿Cómo funcionan las relaciones con usuarios?**
R: 
- `owner_id` en Projects → Creador del proyecto
- `assignee_id` en Tasks → Responsable de la tarea (nullable)
- `created_by` en Tasks → Quién creó la tarea (obligatorio)

**Q: ¿Dónde van los permisos?**
R: Aún no implementado. Se integrará con Entra ID.

---

## 🆘 Recursos
- [Documentación completa](./DOCUMENTACION.md)
- [Colección HTTP](../isc-tmr-backend.http)
- [Swagger](http://localhost:5188/swagger)
- [Scalar](http://localhost:5188/docs)
