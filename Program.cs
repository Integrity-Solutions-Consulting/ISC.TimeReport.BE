using isc_tmr_backend.Infrastructure.Extensions;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddVersioning();
builder.Services.AddInfrastructure();
builder.Services.AddMediatRConfig();
builder.Services.AddDatabase(builder.Configuration);

WebApplication app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(app.Configuration["Docs:ScalarRoute"] ?? "/docs");
}

app.UseHttpsRedirection();
app.Run();