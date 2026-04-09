namespace isc_tmr_backend.Features.Projects;

using FluentValidation;
using isc_tmr_backend.Features.Projects.Application.Commands;
using isc_tmr_backend.Features.Projects.Application.Validators;
using isc_tmr_backend.Features.Projects.Domain;
using isc_tmr_backend.Features.Projects.Endpoint;
using isc_tmr_backend.Features.Projects.Infrastructure.Repositories;

public static class ProjectsModule
{
    public static IServiceCollection AddProjectsServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectsRepository>();
        services.AddScoped<IValidator<CreateProjectCommand>, CreateProjectValidator>();
        services.AddScoped<IValidator<UpdateProjectCommand>, UpdateProjectValidator>();
        return services;
    }

    public static RouteGroupBuilder MapEndpoints(RouteGroupBuilder app)
    {
        app.MapProjectEndpoints();
        return app;
    }
}
