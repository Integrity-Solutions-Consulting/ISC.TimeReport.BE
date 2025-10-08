using isc.time.report.be.api.Extentions;
using isc.time.report.be.application.Interfaces.Repository;
using isc.time.report.be.application.Interfaces.Repository;
using isc.time.report.be.application.Interfaces.Repository.Auth;
using isc.time.report.be.application.Interfaces.Service.Auth;
using isc.time.report.be.application.IOC;
using isc.time.report.be.application.Services.Auth;
using isc.time.report.be.application.Utils.Auth;
using isc.time.report.be.domain.Entity.Emails;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Response.Shared;
using isc.time.report.be.infrastructure.IOC;
using isc.time.report.be.infrastructure.Repositories.Auth;
using isc.time.report.be.infrastructure.Repositories.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using Steeltoe.Discovery.Eureka;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
// Add services to the container.

//var externalConfigPath = @"C:\configs\envvars.json";


//if (File.Exists(externalConfigPath))
//{
//    builder.Configuration.AddJsonFile(externalConfigPath, optional: true, reloadOnChange: true);
//}


var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("OrigenEspecificos", policy =>
    {
        policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddEurekaDiscoveryClient();

builder.Services.AddControllers();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        // Configuración de validación del token
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:JWTSecretKey"]))
        };

        // Aquí hacemos que lea el token desde la cookie
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Verificar si viene en la cookie ISC_TOKEN_TIMEREPORT
                if (context.Request.Cookies.TryGetValue("ISC_TOKEN_TIMEREPORT", out var cookieValue))
                {
                    context.Token = cookieValue;
                }
                return Task.CompletedTask;
            }
        };
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Time Report BE", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT aquí. Ejemplo: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddApplication(builder.Configuration);
builder.Services.AddDbConfiguration(builder.Configuration);
builder.Services.AddInfraestructure(builder.Configuration);



builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        // Cambia la ruta donde se sirve el JSON
        c.RouteTemplate = "timereport/swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Time Report BE v1");
        c.RoutePrefix = "timereport/swagger"; // <-- coincide con tu Gateway
    });
}

app.ConfigureExcepcionHandler();

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("OrigenEspecificos");

app.MapControllers();

app.Run();
