namespace isc_tmr_backend.Infrastructure.Presentation;

// Request con parámetros de paginación
public record RequestWithPagination(
    int Page = 1,
    int Take = 10
);