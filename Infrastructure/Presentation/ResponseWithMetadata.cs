namespace isc_tmr_backend.Infrastructure.Presentation;

// Wrapper global de respuestas exitosas
// T es el tipo de dato que retorna cada endpoint

public record ResponseWithMetadata<T>
{
    public T Data { get; init; } = default!;
    public ResponseMetadata? Metadata { get; init; } = default!; //  (Null-forgiving operator) ! : Le dice al compilador: "Sé que estoy asignando un nulo a una propiedad que no debería ser nula, pero confía en mí, alguien le dará un valor real antes de que se use".
}
