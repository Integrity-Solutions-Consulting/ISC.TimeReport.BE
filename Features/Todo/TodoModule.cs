namespace isc_tmr_backend.Features.Todo;



public static class TodoModule
{
    public static IServiceCollection AddTodoServices(this IServiceCollection services)
    {

        return services;
    }
    
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = TodoController.Create(app); // ← delega la creación del grupo

        TodoController.MapTodoEndpoints(group);

        return app;
    }
}