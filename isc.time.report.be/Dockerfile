# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de solución y proyectos
COPY ISC.TimeReport.sln ./
COPY ISC.TimeReport.Domain/ISC.TimeReport.Domain.csproj ISC.TimeReport.Domain/
COPY ISC.TimeReport.Application/ISC.TimeReport.Application.csproj ISC.TimeReport.Application/
COPY ISC.TimeReport.Infrastructure/ISC.TimeReport.Infrastructure.csproj ISC.TimeReport.Infrastructure/
COPY ISC.TimeReport.API/ISC.TimeReport.API.csproj ISC.TimeReport.API/

# Restaurar dependencias
RUN dotnet restore ISC.TimeReport.sln

# Copiar el resto del código fuente
COPY . .

# Publicar en modo Release
WORKDIR /src/ISC.TimeReport.API
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Variables y puertos
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ISC.TimeReport.API.dll"]
