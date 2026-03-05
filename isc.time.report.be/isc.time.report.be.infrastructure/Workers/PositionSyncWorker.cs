using System.Text.Json;
using isc.time.report.be.infrastructure.Repositories.Sync;
using Microsoft.Extensions.Configuration;   
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Workers
{
    public class PositionSyncWorker : BackgroundService
    {
        private readonly ILogger<PositionSyncWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _postgresConnection;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

        public PositionSyncWorker(
            ILogger<PositionSyncWorker> logger,
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _postgresConnection = configuration.GetConnectionString("PostgreSQL")!;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PositionSyncWorker iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessPendingAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error general en PositionSyncWorker.");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("PositionSyncWorker detenido.");
        }

        private async Task ProcessPendingAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider
                                  .GetRequiredService<OutboxPositionRepository>();

            var pendientes = await repository.GetPendingOutboxPositionsAsync();

            if (!pendientes.Any()) return;

            _logger.LogInformation(
                "PositionSyncWorker: procesando {Count} registro(s) pendiente(s).",
                pendientes.Count);

            await using var pgConnection = new NpgsqlConnection(_postgresConnection);
            await pgConnection.OpenAsync(stoppingToken);

            foreach (var registro in pendientes)
            {
                try
                {
                    switch (registro.Operation.Trim())
                    {
                        case "I":
                        case "U":
                            await UpsertPositionAsync(pgConnection, registro.PayloadJson!);
                            break;

                        case "D":
                            await DeletePositionAsync(pgConnection, registro.AggregateKey);
                            break;

                        default:
                            _logger.LogWarning(
                                "Operación desconocida '{Op}' en OutboxId {Id}.",
                                registro.Operation, registro.OutboxId);
                            break;
                    }

                    await repository.MarkAsProcessedAsync(registro.OutboxId);

                    _logger.LogInformation(
                        "OutboxId {Id} procesado correctamente. Operación: {Op}",
                        registro.OutboxId, registro.Operation);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error al procesar OutboxId {Id}. Intento #{Attempt}.",
                        registro.OutboxId, registro.Attempts + 1);

                    await repository.MarkAsFailedAsync(registro.OutboxId, ex.Message);
                }
            }
        }

        private async Task UpsertPositionAsync(NpgsqlConnection conn, string payloadJson)
        {
            var data = JsonSerializer.Deserialize<JsonElement>(payloadJson);

            const string sql = """
                INSERT INTO administration."position"
                    (id, name, description, status,
                     creation_user, modification_user,
                     creation_date, modification_date,
                     creation_ip, modification_ip)
                VALUES
                    (@id, @name, @description, @status,
                     @creation_user, @modification_user,
                     @creation_date, @modification_date,
                     @creation_ip, @modification_ip)
                ON CONFLICT (id) DO UPDATE SET
                    name              = EXCLUDED.name,
                    description       = EXCLUDED.description,
                    status            = EXCLUDED.status,
                    modification_user = EXCLUDED.modification_user,
                    modification_date = EXCLUDED.modification_date,
                    modification_ip   = EXCLUDED.modification_ip;
                """;

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", data.GetProperty("id").GetInt32());
            cmd.Parameters.AddWithValue("@name", data.GetProperty("name").GetString() ?? "");
            cmd.Parameters.AddWithValue("@description", data.GetProperty("description").GetString() ?? "");
            cmd.Parameters.AddWithValue("@status", GetStatusValue(data));
            cmd.Parameters.AddWithValue("@creation_user", data.GetProperty("creation_user").GetString() ?? "");
            cmd.Parameters.AddWithValue("@modification_user", GetStringOrNull(data, "modification_user"));
            cmd.Parameters.AddWithValue("@creation_date", data.GetProperty("creation_date").GetDateTime());
            cmd.Parameters.AddWithValue("@modification_date", GetDateOrNull(data, "modification_date"));
            cmd.Parameters.AddWithValue("@creation_ip", data.GetProperty("creation_ip").GetString() ?? "");
            cmd.Parameters.AddWithValue("@modification_ip", GetStringOrNull(data, "modification_ip"));

            await cmd.ExecuteNonQueryAsync();
        }

        private async Task DeletePositionAsync(NpgsqlConnection conn, int positionId)
        {
            const string sql = """
                DELETE FROM administration."position" WHERE id = @id;
                """;

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", positionId);
            await cmd.ExecuteNonQueryAsync();
        }

        private static object GetStringOrNull(JsonElement data, string key)
        {
            if (data.TryGetProperty(key, out var val) && val.ValueKind != JsonValueKind.Null)
                return val.GetString() ?? (object)DBNull.Value;
            return DBNull.Value;
        }

        private static object GetDateOrNull(JsonElement data, string key)
        {
            if (data.TryGetProperty(key, out var val) && val.ValueKind != JsonValueKind.Null)
                return val.GetDateTime();
            return DBNull.Value;
        }

        private static int GetStatusValue(JsonElement data)
        {
            // Intentar obtener la propiedad "status"
            if (!data.TryGetProperty("status", out var statusProp))
                throw new InvalidOperationException("La propiedad 'status' no existe en el payload.");

            // Determinar el tipo y convertir
            return statusProp.ValueKind switch
            {
                JsonValueKind.Number => statusProp.GetInt32(),           // Si es número, lo usa directamente
                JsonValueKind.True => 1,                                // Si es true, devuelve 1
                JsonValueKind.False => 0,                                // Si es false, devuelve 0
                _ => throw new InvalidOperationException($"Tipo no soportado para 'status': {statusProp.ValueKind}")
            };
        }
    }
}