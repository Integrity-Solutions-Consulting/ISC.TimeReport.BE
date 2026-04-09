namespace isc_tmr_backend.Infrastructure.Presentation;

public record PagedResult<T>(
    IEnumerable<T> Data,
    int TotalCount
);
