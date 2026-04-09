namespace isc_tmr_backend.Infrastructure.Presentation;

using System.Net;

public record ErrorResponse(
    string Message,
    List<string> Errors,
    HttpStatusCode Status,
    string TraceId,
    DateTime Timestamp
);
