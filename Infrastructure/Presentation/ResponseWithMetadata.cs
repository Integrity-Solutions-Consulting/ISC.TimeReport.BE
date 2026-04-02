namespace isc_tmr_backend.Infrastructure.Presentation;

// Wrapper global de respuestas exitosas
// T es el tipo de dato que retorna cada endpoint

public record ResponseWithMetadata<T>(
    T Data,
    ResponseMetadata? Meta = null,
    string? Message = null,
    int? Code = null    
);
