namespace isc_tmr_backend.Infrastructure.Presentation;

// Request con parámetros de paginación
public record RequestPagination
{
    public int Page { get; init; } = 1;
    public int Take { get; init; } = 10;
};