namespace isc_tmr_backend.Infrastructure.Presentation;

// Metadata de paginación que acompaña las respuestas de listas
public record PaginationMetadata(
    int Page,
    int Take,
    int Count,
    int Total,
    bool HasPreviousPage,
    bool HasNextPage
);
    