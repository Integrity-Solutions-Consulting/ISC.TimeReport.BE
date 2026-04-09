namespace isc_tmr_backend.Features.Tasks;

using FluentValidation;
using isc_tmr_backend.Features.Tasks.Application.Commands;
using isc_tmr_backend.Features.Tasks.Application.Validators;
using isc_tmr_backend.Features.Tasks.Domain;
using isc_tmr_backend.Features.Tasks.Endpoint;
using isc_tmr_backend.Features.Tasks.Infrastructure.Repositories;

public static class TasksModule
{
    public static IServiceCollection AddTasksServices(this IServiceCollection services)
    {
        services.AddScoped<ITaskRepository, TasksRepository>();
        services.AddScoped<IValidator<CreateTaskCommand>, CreateTaskValidator>();
        services.AddScoped<IValidator<UpdateTaskCommand>, UpdateTaskValidator>();
        return services;
    }

    public static RouteGroupBuilder MapEndpoints(RouteGroupBuilder app)
    {
        app.MapTaskEndpoints();
        return app;
    }
}
