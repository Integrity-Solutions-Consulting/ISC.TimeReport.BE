namespace isc_tmr_backend.Infrastructure.Presentation;

using System.Net;

public record ResponseMetadata
{
    public HttpStatusCode Status { get; init; }
    public required string Message { get; init; }
    public PaginationMetadata? Pagination { get; init; } = default!;
};
