// namespace isc_tmr_backend.Infrastructure.Extensions;

// using FluentResults;
// using isc_tmr_backend.Infrastructure.Presentation;
// using Microsoft.AspNetCore.Mvc;

// public static class ResultExtensions
// {
//     // traduce un Result<T> a una respuesta HTTP con formato ApiResponse<T>
//     // equivalente a un @ControllerAdvice en Spring que intercepta el retorno
//     // del @Service y lo envuelve en un ResponseEntity<ApiResponse<T>>
//     // el statusCode es el código HTTP que se retornará si la operación fue exitosa
//     public static IResult ToApiResponse<T>(
//         this Result<T> result,
//         string message,
//         int statusCode = 200)
//     {
//         // si la operación falló retornamos un ProblemDetails RFC 7807
//         // equivalente a lanzar una ResponseStatusException en Spring
//         if (result.IsFailed)
//             return Results.Problem(
//                 detail: string.Join(", ", result.Errors.Select(e => e.Message)),
//                 statusCode: StatusCodes.Status400BadRequest
//             );

//         // si fue exitosa envolvemos el resultado en ApiResponse<T>
//         // usando el factory method Success que definimos antes
//         var response = ApiResponse<T>.Success(result.Value, message, statusCode);

//         // retornamos el código HTTP correcto con el wrapper
//         return statusCode switch
//         {
//             201 => Results.Created(string.Empty, response),
//             204 => Results.NoContent(),
//             _   => Results.Ok(response)
//         };
//     }

//     // sobrecarga para respuestas paginadas
//     // equivalente a ResponseEntity<Page<T>> en Spring Data
//     public static IResult ToPagedApiResponse<T>(
//         this Result<T> result,
//         string message,
//         int page,
//         int pageSize,
//         int totalItems)
//     {
//         if (result.IsFailed)
//             return Results.Problem(
//                 detail: string.Join(", ", result.Errors.Select(e => e.Message)),
//                 statusCode: StatusCodes.Status400BadRequest
//             );

//         var response = ApiResponse<T>.Paginated(
//             result.Value,
//             message,
//             page,
//             pageSize,
//             totalItems
//         );

//         return Results.Ok(response);
//     }
// }