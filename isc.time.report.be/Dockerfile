# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de solución y proyectos
COPY isc.time.report.be.sln ./
COPY isc.time.report.be.api/isc.time.report.be.api.csproj isc.time.report.be.api/
COPY isc.time.report.be.application/isc.time.report.be.application.csproj isc.time.report.be.application/
COPY isc.time.report.be.domain/isc.time.report.be.domain.csproj isc.time.report.be.domain/
COPY isc.time.report.be.infrastructure/isc.time.report.be.infrastructure.csproj isc.time.report.be.infrastructure/

# Restaurar dependencias
RUN dotnet restore

# Copiar todo el código restante
COPY . .

# Publicar en modo Release
WORKDIR /src/isc.time.report.be.api
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Configurar entorno
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "isc.time.report.be.api.dll"]
