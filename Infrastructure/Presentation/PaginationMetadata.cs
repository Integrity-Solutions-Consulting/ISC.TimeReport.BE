namespace isc_tmr_backend.Infrastructure.Presentation;

// Metadata de paginación que acompaña las respuestas de listas
public record PaginationMetadata
{
    public int Page { get; init; }
    public int Take { get; init; }
    public int Count { get; init; }
    public int Total { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
};
    