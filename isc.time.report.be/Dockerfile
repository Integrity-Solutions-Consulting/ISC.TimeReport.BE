FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiamos solo el archivo del proyecto primero para restaurar dependencias
# Esto permite que Docker cachee las librerías si no cambian
COPY ["isc.time.report.be/isc.time.report.be.api.csproj", "isc.time.report.be/"]

# 2. Ejecutamos el restore apuntando específicamente al proyecto
RUN dotnet restore "isc.time.report.be/isc.time.report.be.api.csproj"

# 3. Ahora copiamos el resto del código
COPY . .

# 4. Cambiamos el directorio de trabajo a donde está el proyecto antes de publicar
WORKDIR "/src/isc.time.report.be"
RUN dotnet publish "isc.time.report.be.api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# --- Etapa Final ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# .NET 8 escucha en el puerto 8080 por defecto
EXPOSE 8080
ENTRYPOINT ["dotnet", "isc.time.report.be.api.dll"]
