# Etapa 1 Build
FROM mcr.microsoft.comdotnetsdk8.0 AS build
WORKDIR src

# Copiar solución y proyectos
COPY ISC.TimeReport.sln .
COPY ISC.TimeReport.DomainISC.TimeReport.Domain.csproj ISC.TimeReport.Domain
COPY ISC.TimeReport.ApplicationISC.TimeReport.Application.csproj ISC.TimeReport.Application
COPY ISC.TimeReport.InfrastructureISC.TimeReport.Infrastructure.csproj ISC.TimeReport.Infrastructure
COPY ISC.TimeReport.APIISC.TimeReport.API.csproj ISC.TimeReport.API

# Restaurar dependencias
RUN dotnet restore

# Copiar el resto de los archivos
COPY . .

# Publicar en modo Release
WORKDIR srcISC.TimeReport.API
RUN dotnet publish -c Release -o apppublish

# Etapa 2 Runtime
FROM mcr.microsoft.comdotnetaspnet8.0 AS final
WORKDIR app
COPY --from=build apppublish .

ENV ASPNETCORE_URLS=http+8080
EXPOSE 8080

ENTRYPOINT [dotnet, ISC.TimeReport.API.dll]
