namespace isc_tmr_backend.Infrastructure.Presentation;

using System.Net;

public record ResponseMetadata(
    string Message,
    HttpStatusCode Status,
    PaginationMetadata? Pagination = null
);