namespace isc_tmr_backend.Infrastructure.Extensions;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddProblemDetailsConfig(this IServiceCollection services)
    {
        // AddProblemDetails registra el servicio de RFC 7807 globalmente
        // equivalente a configurar @ControllerAdvice en Spring Boot 3
        // todos los errores no controlados seguirán este formato automáticamente
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                // agregamos campos extra al formato RFC 7807
                // equivalente a extender DefaultErrorAttributes en Spring

                // instance muestra la ruta exacta donde ocurrió el error
                // equivalente a request.getRequestURI() en Spring
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                // agregamos el traceId para poder rastrear el error en los logs
                // equivalente al MDC de Spring con el requestId
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                // timestamp para saber cuándo ocurrió el error
                // equivalente al timestamp del formato clásico de Spring
                context.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow.ToString("o");
            };
        });

        return services;
    }

}