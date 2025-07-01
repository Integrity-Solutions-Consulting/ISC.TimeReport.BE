# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solución y proyectos para restore (desde raíz + prefijo isc.time.report.be/)
COPY isc.time.report.be.sln .
COPY isc.time.report.be.api/isc.time.report.be.api.csproj isc.time.report.be.api/isc.time.report.be.api.csproj
COPY isc.time.report.be.application/isc.time.report.be.application.csproj isc.time.report.be.application/isc.time.report.be.application.csproj
COPY isc.time.report.be.domain/isc.time.report.be.domain.csproj isc.time.report.be.domain/isc.time.report.be.domain.csproj
COPY isc.time.report.be.infrastructure/isc.time.report.be.infrastructure.csproj isc.time.report.be.infrastructure/isc.time.report.be.infrastructure.csproj

RUN dotnet restore isc.time.report.be.sln

# Copiar todo el código fuente dentro de esta carpeta
COPY . .

RUN dotnet build isc.time.report.be.sln -c Release -o /app/build

RUN dotnet publish isc.time.report.be.api/isc.time.report.be.api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "isc.time.report.be.api.dll"]
