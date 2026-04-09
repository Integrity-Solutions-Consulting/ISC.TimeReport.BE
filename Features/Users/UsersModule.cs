namespace isc_tmr_backend.Features.Users;

using FluentValidation;
using isc_tmr_backend.Features.Users.Application.Commands;
using isc_tmr_backend.Features.Users.Application.Validators;
using isc_tmr_backend.Features.Users.Domain;
using isc_tmr_backend.Features.Users.Endpoint;
using isc_tmr_backend.Features.Users.Infrastructure.Repositories;

public static class UsersModule
{
    public static IServiceCollection AddUsersServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UsersRepository>();
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserValidator>();
        services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserValidator>();
        return services;
    }

    public static RouteGroupBuilder MapEndpoints(RouteGroupBuilder app)
    {
        app.MapUserEndpoints();
        return app;
    }
}
