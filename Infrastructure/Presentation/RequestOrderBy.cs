namespace isc_tmr_backend.Infrastructure.Presentation;

public enum SortDirection
{
    Asc,
    Desc
}

// Request con parámetros de paginación
public record RequestOrderBy
{
    public string? Sort { get; init; } = default!;
    public SortDirection Order { get; init; } = SortDirection.Asc;
};