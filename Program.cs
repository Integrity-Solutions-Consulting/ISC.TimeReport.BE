using isc_tmr_backend.Infrastructure.Extensions;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddVersioning();
builder.Services.AddInfrastructure();
builder.Services.AddMediatRConfig();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddProblemDetailsConfig();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(app.Configuration["Docs:ScalarRoute"] ?? "/docs");
}

app.UseMapEndpoints();
app.UseHttpsRedirection();
app.UseProblemDetailsConfig();
app.Run();