# Glosario Técnico

Términos y conceptos utilizados en el proyecto.

---

## A

### AbstractValidator
Clase base de FluentValidation para crear validadores. Define reglas de validación para DTOs y Commands.

```csharp
public class MiValidator : AbstractValidator<MiCommand>
{
    public MiValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty();
    }
}
```

### Aggregate
Patrón de DDD donde una raíz de agregado encapsula un conjunto de entidades relacionadas. En nuestro código, cada entidad (Todo, Notification) actúa como su propio agregado.

---

## C

### CQRS (Command Query Responsibility Segregation)
Patrón que separa operaciones de lectura (Queries) de escritura (Commands). Permite optimizar cada lado independientemente.

- **Commands**: `Create*Command`, `Update*Command`, `Delete*Command` - Modifican estado
- **Queries**: `Get*Query` - Solo leen datos

### Command Handler
Maneja la lógica de un Command. Usa MediatR para recibir y procesar commands.

```csharp
public class CreateHandler : IRequestHandler<CreateCommand, Result<Response>>
{
    public async Task<Result<Response>> Handle(CreateCommand cmd, CancellationToken ct)
    {
        // Lógica de negocio
    }
}
```

### Connection String
Cadena de texto que contiene información para conectarse a una base de datos (host, puerto, credenciales, etc.).

---

## D

### DbContext
Clase de Entity Framework Core que representa una sesión con la base de datos. Usamos `WriteDbContext` y `ReadDbContext` para separar operaciones.

### DDD (Domain-Driven Design)
Metodología de diseño de software que enfoca el modelado en el dominio del negocio.

---

## E

### EF Core (Entity Framework Core)
ORM (Object-Relational Mapper) de Microsoft para .NET. Permite trabajar con bases de datos usando objetos .NET.

### Endpoint
Punto de entrada de la API HTTP. Define una ruta URL y qué sucede cuando se accede a ella.

```csharp
group.MapGet("/recurso", GetRecurso)
    .WithName("GetRecurso")
    .WithTags("Recurso");
```

### Extension Method
Método estático que agrega funcionalidad a un tipo existente sin modificarlo.

```csharp
public static IServiceCollection AddX(this IServiceCollection services)
{
    services.AddScoped<IX, X>();
    return services;
}
```

---

## F

### FluentResults
Biblioteca para manejo de resultados que reemplaza excepciones para lógica de negocio. Retorna `Result<T>` o `Result`.

```csharp
Result.Ok(value);    // Éxito
Result.Fail(error);  // Fracaso
result.IsSuccess;    // Verificar
result.IsFailed;     // Verificar
```

### FluentValidation
Biblioteca para validación declarativa de objetos.

```csharp
RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
```

---

## G

### Guid
Tipo de identificador único global. Genera identificadores únicos como `550e8400-e29b-41d4-a716-446655440000`.

---

## I

### IRequest / IRequestHandler
Interfaces de MediatR para implementar el patrón Mediator.

- `IRequest<T>`: Representa un mensaje/ comando
- `IRequestHandler<T>`: Maneja ese mensaje

---

## M

### MediatR
Biblioteca que implementa el patrón Mediator. Desacopla el código que envía mensajes del código que los maneja.

### Migration
Archivo que describe cambios en el esquema de la base de datos. EF Core las ejecuta para mantener la DB sincronizada.

---

## P

### Primary Constructor
Sintaxis de C# 12 que permite definir parámetros de constructor directamente en la declaración de la clase.

```csharp
public class MiClase(string nombre) 
{
    public string Nombre { get; } = nombre;
}
```

### ProblemDetails
Estándar RFC 7807 para representar errores HTTP de manera estructurada.

---

## Q

### Query Handler
Maneja la lógica de una Query. Similar al Command Handler pero solo para lecturas.

---

## R

### Record
Tipo de C# diseñado para almacenar datos inmutables. Comúnmente usado para DTOs y Commands.

```csharp
public record MiDto(string Nombre, int Valor);
public record MiCommand(MiRequest Request) : IRequest<Result<MiResponse>>;
```

### Repository
Patrón que abstrae el acceso a datos. Define una interfaz pública para operaciones de persistencia.

### Result
Tipo de FluentResults que representa el éxito o fracaso de una operación.

---

## S

### ServiceCollection
Colección de servicios registrados en ASP.NET Core. Se configura en `Program.cs` o extensiones.

### ServiceRegistration
Proceso de agregar servicios al contenedor de dependencias.

---

## T

### Tracking
Concepto de EF Core donde las entidades se monitorean para cambios. `AsNoTracking()` desactiva esto para mejor rendimiento en lecturas.

---

## V

### Validation Problem
Respuesta HTTP 422 cuando la validación de entrada falla. Estándar de ASP.NET Core.

---

## W

### WriteDbContext
DbContext configurado para operaciones de escritura. Tiene tracking habilitado.

---

## Recursos Adicionales

- [CQRS Pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [FluentValidation](https://docs.fluentvalidation.net/)
- [FluentResults](https://github.com/altmann/FluentResults)
- [MediatR](https://github.com/mjbraganza/MediatR)
- [EF Core](https://docs.microsoft.com/en-us/ef/core/)
