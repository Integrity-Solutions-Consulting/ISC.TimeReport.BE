namespace isc_tmr_backend.Infrastructure.Extensions;

using isc_tmr_backend.Infrastructure.Presentation;
using FluentResults;
using Microsoft.AspNetCore.Http;
using System.Net;

public static class ResultExtensions
{
    public static IResult ToSuccessResponse<T>(this Result<T> result, string message, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (result.IsFailed) return result.ToErrorResponse(HttpStatusCode.BadRequest);

        ResponseMetadata metadata = new()
        {
            Message = message,
            Status = statusCode,
        };

        ResponseWithMetadata<T> response = new()
        {
            Data = result.Value,
            Metadata = metadata
        };

        return statusCode switch
        {
            HttpStatusCode.Created => Results.Created(string.Empty, response),
            HttpStatusCode.NoContent => Results.NoContent(),
            _ => Results.Ok(response)
        };
    }

    public static IResult ToSuccessResponse<T>(this Result<T> result, string message, PaginationMetadata pagination, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (result.IsFailed) return result.ToErrorResponse(HttpStatusCode.BadRequest);

        ResponseMetadata metadata = new()
        {
            Message = message,
            Status = statusCode,
            Pagination = pagination
        };

        ResponseWithMetadata<T> response = new()
        {
            Data = result.Value,
            Metadata = metadata
        };

        return Results.Ok(response);
    }

    public static IResult ToErrorResponse<T>(this Result<T> result, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        // var errorResponse = new ErrorResponse(
        //     Message: "Operation failed",
        //     Errors: [.. result.Errors.Select(e => e.Message)],
        //     Status: statusCode,
        //     TraceId: httpContextAccessor?.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString(),
        //     Timestamp: DateTime.UtcNow
        // );

        return statusCode switch
        {
            HttpStatusCode.NotFound => Results.Problem(
                detail: string.Join(", ", result.Errors.Select(e => e.Message)),
                statusCode: StatusCodes.Status404NotFound
            ),
            HttpStatusCode.Unauthorized => Results.Problem(
                detail: string.Join(", ", result.Errors.Select(e => e.Message)),
                statusCode: StatusCodes.Status401Unauthorized
            ),
            HttpStatusCode.Forbidden => Results.Problem(
                detail: string.Join(", ", result.Errors.Select(e => e.Message)),
                statusCode: StatusCodes.Status403Forbidden
            ),
            _ => Results.Problem(
                detail: string.Join(", ", result.Errors.Select(e => e.Message)),
                statusCode: (int)statusCode
            )
        };
    }

    public static IResult ToErrorResponse(this Result result, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return Results.Problem(
            detail: string.Join(", ", result.Errors.Select(e => e.Message)),
            statusCode: (int)statusCode
        );
    }

    // private static IHttpContextAccessor? httpContextAccessor;
    // public static void SetHttpContextAccessor(IHttpContextAccessor accessor) => httpContextAccessor = accessor;

    public static IResult ToPagedResponse<T>(this Result<PagedResult<T>> result, string message, int page, int take, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (result.IsFailed) return result.ToErrorResponse(HttpStatusCode.BadRequest);

        var pagination = new PaginationMetadata
        {
            Page = page,
            Take = take,
            Count = result.Value.Data.Count(),
            Total = result.Value.TotalCount,
            HasPreviousPage = page > 1,
            HasNextPage = (page * take) < result.Value.TotalCount
        };

        ResponseMetadata metadata = new()
        {
            Message = message,
            Status = statusCode,
            Pagination = pagination
        };

        ResponseWithMetadata<IEnumerable<T>>response = new()
        {
            Data = result.Value.Data,
            Metadata = metadata,
        };

        return Results.Ok(response);
    }
}
