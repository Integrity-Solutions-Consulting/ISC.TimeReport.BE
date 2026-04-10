using isc_tmr_backend.Infrastructure.Extensions;
using isc_tmr_backend.Features.Auth;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddVersioning();
builder.Services.AddMediatRConfig();
builder.Services.AddProblemDetailsConfig();
builder.Services.AddReadDatabase(builder.Configuration);
builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddWriteDatabase(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(app.Configuration["Docs:ScalarRoute"] ?? "/docs");
}

app.UseMapEndpoints();
app.UseAuthMiddleware();
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.Run();
